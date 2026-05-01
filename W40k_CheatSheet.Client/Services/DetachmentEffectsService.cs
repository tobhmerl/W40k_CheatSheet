using System.Net.Http.Json;
using W40k_CheatSheet.Client.Models;

namespace W40k_CheatSheet.Client.Services;

public class DetachmentEffectsService
{
    private readonly HttpClient _http;
    private List<DetachmentEffectDefinition>? _definitions;

    public DetachmentEffectsService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<DetachmentEffectDefinition>> GetAllAsync()
    {
        _definitions ??= await _http.GetFromJsonAsync<List<DetachmentEffectDefinition>>("data/detachment_effects.json") ?? [];
        return _definitions;
    }

    public async Task<List<DetachmentEffect>> GetEffectsForDetachment(string? detachment)
    {
        if (string.IsNullOrEmpty(detachment)) return [];
        var all = await GetAllAsync();
        var match = all.FirstOrDefault(d => d.Detachment.Equals(detachment, StringComparison.OrdinalIgnoreCase));
        return match?.Effects ?? [];
    }
}
