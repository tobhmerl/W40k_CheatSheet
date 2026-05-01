using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using W40k_CheatSheet.Client.Models;

namespace W40k_CheatSheet.Client.Services;

public class DetachmentEffectsService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;
    private List<DetachmentEffectDefinition>? _definitions;
    private const string StorageKey = "detachment-effects-custom";

    public DetachmentEffectsService(HttpClient http, IJSRuntime js)
    {
        _http = http;
        _js = js;
    }

    public async Task<List<DetachmentEffectDefinition>> GetAllAsync()
    {
        if (_definitions is not null) return _definitions;

        // Try localStorage first (admin overrides)
        try
        {
            var custom = await _js.InvokeAsync<string?>("localStorage.getItem", StorageKey);
            if (!string.IsNullOrEmpty(custom))
            {
                _definitions = JsonSerializer.Deserialize<List<DetachmentEffectDefinition>>(custom) ?? [];
                return _definitions;
            }
        }
        catch { /* localStorage unavailable */ }

        // Fall back to static JSON
        _definitions = await _http.GetFromJsonAsync<List<DetachmentEffectDefinition>>("data/detachment_effects.json") ?? [];
        return _definitions;
    }

    public async Task<List<DetachmentEffect>> GetEffectsForDetachment(string? detachment)
    {
        if (string.IsNullOrEmpty(detachment)) return [];
        var all = await GetAllAsync();
        var match = all.FirstOrDefault(d => d.Detachment.Equals(detachment, StringComparison.OrdinalIgnoreCase));
        return match?.Effects ?? [];
    }

    public async Task SaveAsync(List<DetachmentEffectDefinition> definitions)
    {
        _definitions = definitions;
        var json = JsonSerializer.Serialize(definitions);
        await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
    }

    public void InvalidateCache() => _definitions = null;
}
