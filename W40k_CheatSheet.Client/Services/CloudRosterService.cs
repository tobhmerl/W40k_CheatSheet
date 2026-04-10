using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;

namespace W40k_CheatSheet.Client.Services;

public class CloudRosterService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;
    private readonly string _supabaseUrl;
    private readonly string _anonKey;
    private string? _accessToken;
    private string? _refreshToken;

    public bool IsLoggedIn { get; private set; }
    public string? Username { get; private set; }
    public string? ErrorMessage { get; private set; }

    public CloudRosterService(HttpClient http, IJSRuntime js, string supabaseUrl, string anonKey)
    {
        _http = http;
        _js = js;
        _supabaseUrl = supabaseUrl.TrimEnd('/');
        _anonKey = anonKey;
    }

    // ── Session persistence ──

    public async Task TryRestoreSessionAsync()
    {
        try
        {
            var token = await _js.InvokeAsync<string?>("localStorage.getItem", "cloud_access_token");
            var refresh = await _js.InvokeAsync<string?>("localStorage.getItem", "cloud_refresh_token");
            var username = await _js.InvokeAsync<string?>("localStorage.getItem", "cloud_username");

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(username))
                return;

            // Check if the access token is still valid (not expired)
            if (!IsTokenExpired(token))
            {
                _accessToken = token;
                _refreshToken = refresh;
                IsLoggedIn = true;
                Username = username;
                return;
            }

            // Access token expired — try refresh
            if (!string.IsNullOrEmpty(refresh) && await RefreshSessionAsync(refresh))
            {
                Username = username;
                await PersistSession(username);
                return;
            }

            // Refresh failed — clear stale data
            await ClearPersistedSession();
        }
        catch { }
    }

    private async Task<bool> RefreshSessionAsync(string refreshToken)
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post,
                $"{_supabaseUrl}/auth/v1/token?grant_type=refresh_token");
            request.Headers.Add("apikey", _anonKey);
            request.Content = JsonContent.Create(new { refresh_token = refreshToken });

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode) return false;

            var body = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(body);

            _accessToken = doc.RootElement.GetProperty("access_token").GetString();
            if (doc.RootElement.TryGetProperty("refresh_token", out var rt))
                _refreshToken = rt.GetString();

            if (string.IsNullOrEmpty(_accessToken)) return false;

            IsLoggedIn = true;
            return true;
        }
        catch { return false; }
    }

    private async Task PersistSession(string username)
    {
        try
        {
            await _js.InvokeVoidAsync("localStorage.setItem", "cloud_access_token", _accessToken ?? "");
            await _js.InvokeVoidAsync("localStorage.setItem", "cloud_refresh_token", _refreshToken ?? "");
            await _js.InvokeVoidAsync("localStorage.setItem", "cloud_username", username);
        }
        catch { }
    }

    private async Task ClearPersistedSession()
    {
        try
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", "cloud_access_token");
            await _js.InvokeVoidAsync("localStorage.removeItem", "cloud_refresh_token");
            await _js.InvokeVoidAsync("localStorage.removeItem", "cloud_username");
        }
        catch { }
    }

    private static bool IsTokenExpired(string token)
    {
        try
        {
            var parts = token.Split('.');
            if (parts.Length < 2) return true;
            var payload = parts[1].Replace('-', '+').Replace('_', '/');
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }
            var bytes = Convert.FromBase64String(payload);
            using var doc = JsonDocument.Parse(bytes);
            if (doc.RootElement.TryGetProperty("exp", out var exp))
            {
                var expiry = DateTimeOffset.FromUnixTimeSeconds(exp.GetInt64());
                return expiry < DateTimeOffset.UtcNow.AddMinutes(-1); // 1 min buffer
            }
        }
        catch { }
        return true;
    }

    // ── Auth ──

    public async Task<bool> IsUsernameAvailableAsync(string username)
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get,
                $"{_supabaseUrl}/rest/v1/profiles?username=eq.{Uri.EscapeDataString(username)}&select=username");
            request.Headers.Add("apikey", _anonKey);

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode) return false;

            var body = await response.Content.ReadAsStringAsync();
            return body == "[]";
        }
        catch { return false; }
    }

    public async Task<bool> RegisterAsync(string email, string password, string username)
    {
        ErrorMessage = null;
        try
        {
            if (!await IsUsernameAvailableAsync(username))
            {
                ErrorMessage = "Username already taken.";
                return false;
            }

            using var request = new HttpRequestMessage(HttpMethod.Post, $"{_supabaseUrl}/auth/v1/signup");
            request.Headers.Add("apikey", _anonKey);
            request.Content = JsonContent.Create(new { email, password, data = new { username } });

            var response = await _http.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = $"Registration failed: {ParseError(body)}";
                return false;
            }

            if (!await LoginAsync(email, password))
                return false;

            await InsertProfileAsync(username);
            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Connection error: {ex.Message}";
            return false;
        }
    }

    private async Task InsertProfileAsync(string username)
    {
        try
        {
            var userId = GetUserIdFromToken();
            if (userId is null) return;

            using var request = BuildRequest(HttpMethod.Post, "/rest/v1/profiles");
            request.Content = JsonContent.Create(new { user_id = userId, username });
            await _http.SendAsync(request);
        }
        catch { }
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        ErrorMessage = null;
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post,
                $"{_supabaseUrl}/auth/v1/token?grant_type=password");
            request.Headers.Add("apikey", _anonKey);
            request.Content = JsonContent.Create(new { email, password });

            var response = await _http.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Invalid email or password.";
                return false;
            }

            using var doc = JsonDocument.Parse(body);
            _accessToken = doc.RootElement.GetProperty("access_token").GetString();
            if (doc.RootElement.TryGetProperty("refresh_token", out var rt))
                _refreshToken = rt.GetString();

            if (string.IsNullOrEmpty(_accessToken))
            {
                ErrorMessage = "No token received.";
                return false;
            }

            // Extract username from user metadata, fallback to profiles table, then email
            string? username = null;
            if (doc.RootElement.TryGetProperty("user", out var user) &&
                user.TryGetProperty("user_metadata", out var meta) &&
                meta.TryGetProperty("username", out var uname))
            {
                username = uname.GetString();
            }

            username ??= await FetchUsernameAsync();
            username ??= email;

            IsLoggedIn = true;
            Username = username;
            await PersistSession(username);
            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Connection error: {ex.Message}";
            return false;
        }
    }

    private async Task<string?> FetchUsernameAsync()
    {
        try
        {
            var userId = GetUserIdFromToken();
            if (userId is null) return null;

            using var request = BuildRequest(HttpMethod.Get,
                $"/rest/v1/profiles?user_id=eq.{userId}&select=username");

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;

            var body = await response.Content.ReadAsStringAsync();
            using var arr = JsonDocument.Parse(body);
            var first = arr.RootElement.EnumerateArray().FirstOrDefault();
            if (first.ValueKind != JsonValueKind.Undefined &&
                first.TryGetProperty("username", out var un))
                return un.GetString();
        }
        catch { }
        return null;
    }

    public async Task LogoutAsync()
    {
        IsLoggedIn = false;
        Username = null;
        _accessToken = null;
        _refreshToken = null;
        await ClearPersistedSession();
    }

    // ── Roster CRUD ──

    public record CloudRosterMeta(Guid Id, string Name, string Faction, string Detachment, int Points, DateTime LastModified);

    public async Task<List<CloudRosterMeta>> ListRostersAsync()
    {
        try
        {
            using var request = BuildRequest(HttpMethod.Get,
                "/rest/v1/rosters?select=id,name,faction,detachment,points,last_modified&order=last_modified.desc");

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode) return [];

            var items = await response.Content.ReadFromJsonAsync<List<SupabaseRosterRow>>();
            return items?.Select(r => new CloudRosterMeta(
                r.Id, r.Name, r.Faction, r.Detachment, r.Points, r.LastModified
            )).ToList() ?? [];
        }
        catch { return []; }
    }

    public async Task<string?> LoadRosterDataAsync(Guid id)
    {
        try
        {
            using var request = BuildRequest(HttpMethod.Get,
                $"/rest/v1/rosters?id=eq.{id}&select=data_json");

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;

            var items = await response.Content.ReadFromJsonAsync<List<SupabaseRosterRow>>();
            return items?.FirstOrDefault()?.DataJson;
        }
        catch { return null; }
    }

    public async Task<bool> SaveRosterAsync(string name, string faction, string detachment, int points, string dataJson)
    {
        try
        {
            var userId = GetUserIdFromToken();
            if (userId is null) return false;

            using var request = BuildRequest(HttpMethod.Post, "/rest/v1/rosters");
            request.Headers.Add("Prefer", "resolution=merge-duplicates");

            request.Content = JsonContent.Create(new
            {
                user_id = userId,
                name,
                faction,
                detachment,
                points,
                data_json = dataJson,
                last_modified = DateTime.UtcNow.ToString("o")
            });

            var response = await _http.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> DeleteRosterAsync(Guid id)
    {
        try
        {
            using var request = BuildRequest(HttpMethod.Delete,
                $"/rest/v1/rosters?id=eq.{id}");

            var response = await _http.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    // ── Helpers ──

    private HttpRequestMessage BuildRequest(HttpMethod method, string path)
    {
        var request = new HttpRequestMessage(method, $"{_supabaseUrl}{path}");
        request.Headers.Add("apikey", _anonKey);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        return request;
    }

    private string? GetUserIdFromToken()
    {
        if (string.IsNullOrEmpty(_accessToken)) return null;
        try
        {
            var parts = _accessToken.Split('.');
            if (parts.Length < 2) return null;
            var payload = parts[1].Replace('-', '+').Replace('_', '/');
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }
            var bytes = Convert.FromBase64String(payload);
            using var doc = JsonDocument.Parse(bytes);
            return doc.RootElement.GetProperty("sub").GetString();
        }
        catch { return null; }
    }

    private static string ParseError(string body)
    {
        try
        {
            using var doc = JsonDocument.Parse(body);
            if (doc.RootElement.TryGetProperty("msg", out var msg))
                return msg.GetString() ?? body;
            if (doc.RootElement.TryGetProperty("error_description", out var desc))
                return desc.GetString() ?? body;
            if (doc.RootElement.TryGetProperty("message", out var message))
                return message.GetString() ?? body;
        }
        catch { }
        return body;
    }

    private sealed class SupabaseRosterRow
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("faction")]
        public string Faction { get; set; } = "";

        [JsonPropertyName("detachment")]
        public string Detachment { get; set; } = "";

        [JsonPropertyName("points")]
        public int Points { get; set; }

        [JsonPropertyName("data_json")]
        public string DataJson { get; set; } = "";

        [JsonPropertyName("last_modified")]
        public DateTime LastModified { get; set; }
    }
}
