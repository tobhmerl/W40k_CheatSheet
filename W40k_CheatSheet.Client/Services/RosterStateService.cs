using W40k_CheatSheet.Client.Models;

namespace W40k_CheatSheet.Client.Services;

/// <summary>
/// Single source of truth for the currently loaded roster + per-army setup configs.
/// Services and components can share the same dictionary instances and subscribe to
/// <see cref="StateChanged"/> for re-renders.
/// </summary>
public sealed class RosterStateService
{
    public ArmyRoster? Army { get; private set; }
    public string? RawJson { get; private set; }

    public Dictionary<string, AbilitySetupEntry>          AbilityConfigs          { get; }
        = new(StringComparer.Ordinal);
    public Dictionary<string, StratagemSetupEntry>        StratagemConfigs        { get; }
        = new(StringComparer.Ordinal);
    public DetachmentSetupEntry                           DetachmentConfig        { get; private set; } = new();
    public Dictionary<string, DetachmentEffectSetupEntry> DetachmentEffectConfigs { get; }
        = new(StringComparer.Ordinal);
    public Dictionary<string, ArmyRuleSetupEntry>         ArmyRuleConfigs         { get; }
        = new(StringComparer.Ordinal);

    public List<DetachmentEffect> ActiveDetachmentEffects { get; private set; } = [];

    public event Action? StateChanged;

    public void SetArmy(ArmyRoster? army, string? rawJson)
    {
        Army = army;
        RawJson = rawJson;
        StateChanged?.Invoke();
    }

    public void SetActiveDetachmentEffects(List<DetachmentEffect> effects)
    {
        ActiveDetachmentEffects = effects ?? [];
        StateChanged?.Invoke();
    }

    public void SetDetachmentConfig(DetachmentSetupEntry cfg)
    {
        DetachmentConfig = cfg ?? new DetachmentSetupEntry();
        StateChanged?.Invoke();
    }

    /// <summary>Wipe all per-army configs (used when loading a fresh roster).</summary>
    public void ClearForNewArmy()
    {
        AbilityConfigs.Clear();
        StratagemConfigs.Clear();
        DetachmentEffectConfigs.Clear();
        ArmyRuleConfigs.Clear();
        DetachmentConfig = new DetachmentSetupEntry();
        ActiveDetachmentEffects = [];
        StateChanged?.Invoke();
    }

    public void NotifyChanged() => StateChanged?.Invoke();
}
