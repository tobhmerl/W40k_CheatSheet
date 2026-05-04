using System.Text.RegularExpressions;
using W40k_CheatSheet.Client.Models;

namespace W40k_CheatSheet.Client.Services;

/// <summary>
/// Stateless resolver for ability / detachment-effect / army-rule semantics.
/// Centralizes all "is X applied / reflected in stats / added to unit" logic
/// so UI code does not duplicate it.
/// </summary>
public sealed class EffectResolverService
{
    public enum WeaponTarget { Both, Ranged, Melee }
    public enum DefensiveAuraType { Invulnerable, FeelNoPain }

    // ── Ability config helpers ──────────────────────────────────────────────

    public static string AbilityKey(AbilityEntry a) => a.Name;

    /// <summary>Both turn flags off => explicitly hidden from the abilities panel.</summary>
    public bool IsHiddenAbility(AbilityEntry a, IReadOnlyDictionary<string, AbilitySetupEntry> configs) =>
        configs.TryGetValue(AbilityKey(a), out var c) && !c.MyTurn && !c.EnemyTurn;

    /// <summary>"Apply to stats directly" — inject the ability onto the unit/weapon row instead of the panel.</summary>
    public bool IsAddedToUnit(AbilityEntry a, IReadOnlyDictionary<string, AbilitySetupEntry> configs) =>
        configs.TryGetValue(AbilityKey(a), out var c) && c.ApplyToStats;

    // ── Pure parsers (description sniffing) ─────────────────────────────────

    public static (string Keyword, WeaponTarget Target)? ParseLeaderWeaponAbility(AbilityEntry a)
    {
        var desc = a.Description;
        if (!desc.Contains("while this model is leading", StringComparison.OrdinalIgnoreCase) &&
            !desc.Contains("while leading", StringComparison.OrdinalIgnoreCase))
            return null;

        var kwMatch = Regex.Match(desc, @"\[([A-Z][A-Z\s\d\+]+)\]");
        if (!kwMatch.Success) return null;

        var keyword = kwMatch.Groups[1].Value.Trim();
        var target = WeaponTarget.Both;
        if (desc.Contains("melee weapons", StringComparison.OrdinalIgnoreCase))
            target = WeaponTarget.Melee;
        else if (desc.Contains("ranged weapons", StringComparison.OrdinalIgnoreCase))
            target = WeaponTarget.Ranged;

        return (keyword, target);
    }

    public static (DefensiveAuraType Type, string Value)? ParseLeaderDefensiveAura(AbilityEntry a)
    {
        var desc = a.Description;
        if (!desc.Contains("while this model is leading", StringComparison.OrdinalIgnoreCase) &&
            !desc.Contains("while leading", StringComparison.OrdinalIgnoreCase))
            return null;

        var invulnMatch = Regex.Match(desc, @"(\d)\+\s*invulnerable\s+save", RegexOptions.IgnoreCase);
        if (invulnMatch.Success)
            return (DefensiveAuraType.Invulnerable, invulnMatch.Groups[1].Value + "+");

        var fnpMatch = Regex.Match(desc, @"feel\s+no\s+pain\s+(\d)\+", RegexOptions.IgnoreCase);
        if (fnpMatch.Success)
            return (DefensiveAuraType.FeelNoPain, fnpMatch.Groups[1].Value + "+");

        return null;
    }

    public static bool IsLeaderAuraDescription(string desc) =>
        desc.Contains("while this model is leading", StringComparison.OrdinalIgnoreCase) ||
        desc.Contains("while leading", StringComparison.OrdinalIgnoreCase) ||
        desc.Contains("while the bearer is leading", StringComparison.OrdinalIgnoreCase) ||
        desc.Contains("is leading this unit", StringComparison.OrdinalIgnoreCase);

    public static string? ParseBearerKeyword(AbilityEntry a)
    {
        var desc = a.Description;
        if (IsLeaderAuraDescription(desc)) return null;
        var match = Regex.Match(desc,
            @"(?:bearer|model|unit)\s+has\s+the\s+(\w+(?:\s\w+)*?)\s+ability",
            RegexOptions.IgnoreCase);
        return match.Success ? match.Groups[1].Value.Trim().ToUpperInvariant() : null;
    }

    // ── Detachment-effect resolution ────────────────────────────────────────

    /// <summary>True if any active detachment effect is reflected in unit/weapon stats and the user opted in.</summary>
    public bool IsDetachmentRuleReflectedInStats(
        ArmyRoster? army,
        IReadOnlyList<DetachmentEffect> activeEffects,
        Func<int, bool> isEffectActive)
    {
        if (army is null || string.IsNullOrEmpty(army.DetachmentRule)) return false;
        for (int i = 0; i < activeEffects.Count; i++)
        {
            if (activeEffects[i].ReflectedInStats && isEffectActive(i))
                return true;
        }
        return false;
    }

    /// <summary>True if the unit currently benefits from a detachment-driven BS/WS bonus (and the user opted in).</summary>
    public bool UnitHasDetachmentHitBonus(
        ArmyRoster? army,
        UnitEntry unit,
        IReadOnlyList<DetachmentEffect> activeEffects,
        Func<int, bool> isEffectActive,
        Func<UnitEntry, bool> isKilled)
    {
        if (army is null) return false;
        for (int i = 0; i < activeEffects.Count; i++)
        {
            var e = activeEffects[i];
            if (!e.ReflectedInStats || !isEffectActive(i)) continue;
            if (e.Stat != "bs" && e.Stat != "ws") continue;
            if (EvaluateCondition(e.Condition, unit, isKilled))
                return true;
        }
        return false;
    }

    public bool EvaluateCondition(string condition, UnitEntry unit, Func<UnitEntry, bool> isKilled) => condition switch
    {
        "always" => true,
        "has_leader_character" => unit.AttachedLeaders.Any(l =>
            !isKilled(l) &&
            l.Keywords.Any(k => k.Equals("Character", StringComparison.OrdinalIgnoreCase))),
        "charged" => true,
        "battle_shocked_and_charged" => true,
        _ => false
    };
}
