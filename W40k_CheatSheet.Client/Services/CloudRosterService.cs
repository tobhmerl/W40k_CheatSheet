using System.Net.Http.Json;
using System.Text.Json;

namespace W40k_CheatSheet.Client.Services;

public class CloudRosterService(HttpClient http)
{
    public bool IsLoggedIn { get; private set; }
    public string? Username { get; private set; }
    public string? ErrorMessage { get; private set; }

    // ── Auth ──

    public async Task<bool> RegisterAsync(string email, string password)
    {
        ErrorMessage = null;
        try
        {
            var response = await http.PostAsJsonAsync("api/auth/register", new { email, password });
            if (response.IsSuccessStatusCode)
                return await LoginAsync(email, password);

            var body = await response.Content.ReadAsStringAsync();
            ErrorMessage = $"Registration failed: {body}";
            return false;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Connection error: {ex.Message}";
            return false;
        }
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        ErrorMessage = null;
        try
        {
            var response = await http.PostAsJsonAsync("api/auth/login", new { email, password });
            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Invalid email or password.";
                return false;
            }

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var token = doc.RootElement.GetProperty("accessToken").GetString();
            if (string.IsNullOrEmpty(token))
            {
                ErrorMessage = "No token received.";
                return false;
            }

            http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            IsLoggedIn = true;
            Username = email;
            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Connection error: {ex.Message}";
            return false;
        }
    }

    public void Logout()
    {
        IsLoggedIn = false;
        Username = null;
        http.DefaultRequestHeaders.Authorization = null;
    }

    // ── Roster CRUD ──

    public record CloudRosterMeta(Guid Id, string Name, string Faction, string Detachment, int Points, DateTime LastModified);

    public async Task<List<CloudRosterMeta>> ListRostersAsync()
    {
        try
        {
            return await http.GetFromJsonAsync<List<CloudRosterMeta>>("api/rosters") ?? [];
        }
        catch { return []; }
    }

    public async Task<string?> LoadRosterDataAsync(Guid id)
    {
        try
        {
            var response = await http.GetAsync($"api/rosters/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("dataJson").GetString();
        }
        catch { return null; }
    }

    public async Task<bool> SaveRosterAsync(string name, string faction, string detachment, int points, string dataJson)
    {
        try
        {
            var response = await http.PostAsJsonAsync("api/rosters", new { name, faction, detachment, points, dataJson });
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> DeleteRosterAsync(Guid id)
    {
        try
        {
            var response = await http.DeleteAsync($"api/rosters/{id}");
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }
}
