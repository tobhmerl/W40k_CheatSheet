using System.Text.Json;
using System.Text.RegularExpressions;
using W40k_CheatSheet.Client.Models;

namespace W40k_CheatSheet.Client.Services;

public partial class RosterParserService
{
    private static string SanitizeText(string text) =>
        text.Replace("^^", "").Replace("**", "").Trim();

    public ArmyRoster Parse(string json)
    {
        var root = JsonSerializer.Deserialize<RosterRoot>(json)
            ?? throw new InvalidOperationException("Failed to parse roster JSON.");

        var roster = root.Roster;
        var army = new ArmyRoster
        {
            Name = roster.Name,
            TotalPoints = (int)(roster.Costs.FirstOrDefault(c => c.Name == "pts")?.Value ?? 0),
            PointLimit = (int)(roster.CostLimits.FirstOrDefault(c => c.Name == "pts")?.Value ?? 0)
        };

        foreach (var force in roster.Forces)
        {
            army.Faction = force.CatalogueName;

            foreach (var selection in force.Selections)
            {
                if (selection.Categories.Any(c => c.Name == "Configuration"))
                {
                    if (selection.Name == "Detachment")
                    {
                        var det = selection.Selections.FirstOrDefault();
                        if (det != null)
                        {
                            army.Detachment = det.Name;
                            var rule = det.Rules.FirstOrDefault(r => !r.Hidden);
                            if (rule != null)
                                army.DetachmentRule = SanitizeText(rule.Description);
                        }
                    }
                    continue;
                }

                var unit = ParseUnit(selection);
                army.Units.Add(unit);
            }
        }

        return army;
    }

    private static UnitEntry ParseUnit(Selection selection)
    {
        var unit = new UnitEntry
        {
            Name = selection.Name,
            Points = GetTotalPoints(selection),
            Keywords = selection.Categories
                .Where(c => !c.Name.StartsWith("Faction:"))
                .Select(c => c.Name)
                .Distinct()
                .ToList()
        };

        foreach (var profile in selection.Profiles)
            ProcessProfile(profile, unit);

        foreach (var rule in selection.Rules)
            ProcessRule(rule, unit);

        foreach (var sub in selection.Selections)
            ProcessSubSelection(sub, unit);

        unit.Abilities = unit.Abilities.DistinctBy(a => a.Name).ToList();
        unit.RangedWeapons = unit.RangedWeapons
            .GroupBy(w => w.Name)
            .Select(g => { var w = g.First(); w.ModelsEquipped = g.Sum(x => x.ModelsEquipped); return w; })
            .ToList();
        unit.MeleeWeapons = unit.MeleeWeapons
            .GroupBy(w => w.Name)
            .Select(g => { var w = g.First(); w.ModelsEquipped = g.Sum(x => x.ModelsEquipped); return w; })
            .ToList();
        unit.StatLines = unit.StatLines.DistinctBy(s => s.Name).ToList();

        unit.IsLeader = (unit.Keywords.Contains("Character")
            && unit.Abilities.Any(a => a.Name == "Leader"))
            || unit.Abilities.Any(a => a.Name.Contains("Retinue", StringComparison.OrdinalIgnoreCase));

        unit.Abilities.RemoveAll(a => a.Name == "Leader");

        unit.ModelCount = CountModels(selection);

        // Extract defensive modifiers from abilities
        foreach (var ability in unit.Abilities)
        {
            var dm = DefensiveModifierRegex().Match(ability.Description);
            if (!dm.Success) continue;
            var roll = dm.Groups[2].Value.ToLowerInvariant(); // "hit" or "wound"
            var value = -int.Parse(dm.Groups[1].Value);
            var attackType = ability.Description.Contains("melee attack", StringComparison.OrdinalIgnoreCase) ? "melee"
                           : ability.Description.Contains("ranged attack", StringComparison.OrdinalIgnoreCase) ? "ranged"
                           : "all";
            var condition = "";
            if (ability.Description.Contains("Strength", StringComparison.OrdinalIgnoreCase) &&
                ability.Description.Contains("Toughness", StringComparison.OrdinalIgnoreCase))
                condition = "S > T";
            unit.DefensiveModifiers.Add(new DefensiveModifier
            {
                Roll = roll,
                Value = value,
                AttackType = attackType,
                Condition = condition,
                Source = ability.Name
            });
        }

        // Extract charge effects from abilities
        foreach (var ability in unit.Abilities)
        {
            var cm = ChargeEffectRegex().Match(ability.Description);
            if (!cm.Success) continue;
            // Build a short summary from the description
            var desc = ability.Description;
            var summary = BuildChargeSummary(desc);
            unit.ChargeEffects.Add(new ChargeEffect
            {
                Summary = summary,
                Source = ability.Name,
                Description = desc
            });
        }

        return unit;
    }

    private static int CountModels(Selection selection)
    {
        // Sub-selections with type "model" represent individual models in the unit
        var modelSubs = selection.Selections
            .Where(s => s.Type.Equals("model", StringComparison.OrdinalIgnoreCase))
            .ToList();
        if (modelSubs.Count > 0)
            return modelSubs.Sum(s => s.Number);
        // Fallback: if the selection itself is a single model (e.g. Character)
        return selection.Number > 0 ? selection.Number : 1;
    }

    private static void ProcessProfile(Profile profile, UnitEntry unit, int modelsEquipped = 0)
    {
        if (profile.Hidden)
            return;

        switch (profile.TypeName)
        {
            case "Unit":
                var stat = new StatLine { Name = profile.Name };
                foreach (var c in profile.Characteristics)
                {
                    switch (c.Name)
                    {
                        case "M": stat.M = c.Text; break;
                        case "T": stat.T = c.Text; break;
                        case "SV": stat.SV = c.Text; break;
                        case "W": stat.W = c.Text; break;
                        case "LD": stat.LD = c.Text; break;
                        case "OC": stat.OC = c.Text; break;
                    }
                }
                unit.StatLines.Add(stat);
                break;

            case "Abilities":
                var desc = SanitizeText(profile.Characteristics
                    .FirstOrDefault(c => c.Name == "Description")?.Text ?? "");
                if (profile.Name == "Invulnerable Save")
                {
                    var match = InvulnRegex().Match(desc);
                    unit.InvulnerableSave = match.Success ? match.Groups[1].Value : desc;
                }
                else
                {
                    bool isLeaderAura = desc.Contains("while this model is leading", StringComparison.OrdinalIgnoreCase);

                    var fnpMatch = MatchFnp(desc);
                    if (fnpMatch.Success)
                    {
                        var fnpValue = fnpMatch.Groups[1].Value;
                        if (isLeaderAura)
                            unit.FeelNoPainAura = BetterValue(unit.FeelNoPainAura, fnpValue);
                        else
                            unit.FeelNoPain = BetterValue(unit.FeelNoPain, fnpValue);
                    }

                    var invulnMatch = InvulnRegex().Match(desc);
                    if (invulnMatch.Success)
                    {
                        if (isLeaderAura)
                            unit.InvulnerableSaveAura = BetterValue(unit.InvulnerableSaveAura, invulnMatch.Groups[1].Value);
                        else
                            unit.InvulnerableSave = BetterValue(unit.InvulnerableSave, invulnMatch.Groups[1].Value);
                    }

                    unit.Abilities.Add(new AbilityEntry { Name = profile.Name, Description = desc, Phases = ClassifyPhase(desc) });
                }
                break;

            case "Ranged Weapons":
                var rw = ParseWeapon(profile);
                if (modelsEquipped > 0) rw.ModelsEquipped = modelsEquipped;
                unit.RangedWeapons.Add(rw);
                break;

            case "Melee Weapons":
                var mw = ParseWeapon(profile);
                if (modelsEquipped > 0) mw.ModelsEquipped = modelsEquipped;
                unit.MeleeWeapons.Add(mw);
                break;
        }
    }

    private static WeaponProfile ParseWeapon(Profile profile)
    {
        var weapon = new WeaponProfile { Name = profile.Name };
        foreach (var c in profile.Characteristics)
        {
            switch (c.Name)
            {
                case "Range": weapon.Range = c.Text; break;
                case "A": weapon.A = c.Text; break;
                case "BS" or "WS": weapon.Skill = c.Text; break;
                case "S": weapon.S = c.Text; break;
                case "AP": weapon.AP = c.Text; break;
                case "D": weapon.D = c.Text; break;
                case "Keywords": weapon.Keywords = c.Text; break;
            }
        }
        return weapon;
    }

    private static void ProcessSubSelection(Selection sub, UnitEntry unit, int modelsEquipped = 0)
    {
        if (sub.Name == "Warlord")
        {
            unit.IsWarlord = true;
            return;
        }

        if (sub.Type.Equals("model", StringComparison.OrdinalIgnoreCase) && sub.Number > 0)
            modelsEquipped = sub.Number;

        if (sub.Group == "Enhancements")
        {
            var enhProfile = sub.Profiles.FirstOrDefault(p => p.TypeName == "Abilities");
            if (enhProfile != null)
            {
                var desc = SanitizeText(enhProfile.Characteristics
                    .FirstOrDefault(c => c.Name == "Description")?.Text ?? "");
                unit.Enhancements.Add(enhProfile.Name);
                unit.Abilities.Add(new AbilityEntry { Name = enhProfile.Name, Description = desc, Phases = ClassifyPhase(desc) });
            }
        }

        foreach (var profile in sub.Profiles)
            ProcessProfile(profile, unit, modelsEquipped);

        foreach (var rule in sub.Rules)
            ProcessRule(rule, unit);

        foreach (var nested in sub.Selections)
            ProcessSubSelection(nested, unit, modelsEquipped);
    }

    private static void ProcessRule(Rule rule, UnitEntry unit)
    {
        if (rule.Hidden) return;

        var desc = SanitizeText(rule.Description);

        // Feel No Pain from rules (e.g. C'tan "Feel No Pain 5+")
        var fnpNameMatch = Regex.Match(rule.Name, @"feel no pain (\d\+)", RegexOptions.IgnoreCase);
        if (fnpNameMatch.Success)
        {
            unit.FeelNoPain = BetterValue(unit.FeelNoPain, fnpNameMatch.Groups[1].Value);
            return;
        }
        var fnpMatch = MatchFnp(desc);
        if (fnpMatch.Success && rule.Name.Contains("Feel No Pain", StringComparison.OrdinalIgnoreCase))
        {
            unit.FeelNoPain = BetterValue(unit.FeelNoPain, fnpMatch.Groups[1].Value);
            return;
        }

        // Deadly Demise — store as an ability so it shows in the card
        if (rule.Name.StartsWith("Deadly Demise", StringComparison.OrdinalIgnoreCase))
        {
            unit.Abilities.Add(new AbilityEntry { Name = rule.Name, Description = desc, Phases = GamePhase.All });
            return;
        }

        // Pre-game abilities (Scout, Deep Strike, Infiltrators)
        if (IsPreGameProfileName(rule.Name))
        {
            unit.Abilities.Add(new AbilityEntry { Name = rule.Name, Description = desc, Phases = GamePhase.Move });
            return;
        }
    }

    private static int GetTotalPoints(Selection selection)
    {
        int pts = (int)(selection.Costs.FirstOrDefault(c => c.Name == "pts")?.Value ?? 0);
        foreach (var sub in selection.Selections)
            pts += GetTotalPoints(sub);
        return pts;
    }

    internal static GamePhase ClassifyPhase(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return GamePhase.All;

        var text = description.ToLowerInvariant();

        // ── Tier 1: explicit "<Name> Phase" references are the strongest signal.
        //    When present they override every broad keyword below.
        var explicitPhases = GamePhase.None;

        if (text.Contains("command phase"))
            explicitPhases |= GamePhase.Command;
        if (text.Contains("movement phase"))
            explicitPhases |= GamePhase.Move;
        if (text.Contains("shooting phase"))
            explicitPhases |= GamePhase.Shoot;
        if (text.Contains("charge phase"))
            explicitPhases |= GamePhase.Charge;
        if (text.Contains("fight phase"))
            explicitPhases |= GamePhase.Fight;

        if (explicitPhases != GamePhase.None)
        {
            // Battle-shock / leadership tests are always Command-relevant
            if (text.Contains("battle-shock") || text.Contains("battleshock") ||
                text.Contains("leadership test"))
                explicitPhases |= GamePhase.Command;
            return explicitPhases;
        }

        // ── Tier 2: contextual keywords (no explicit phase was named)
        var phases = GamePhase.None;

        // Command
        if (text.Contains("battle-shock") || text.Contains("battleshock") ||
            text.Contains("leadership test") || text.Contains("battle round"))
            phases |= GamePhase.Command;

        // Movement
        if (text.Contains("normal move") || text.Contains("fall back") ||
            text.Contains("deep strike") || text.Contains("reserves") ||
            text.Contains("reinforcement"))
            phases |= GamePhase.Move;

        // Shooting
        if (text.Contains("selected to shoot") || text.Contains("ballistic skill") ||
            text.Contains("ranged attack"))
            phases |= GamePhase.Shoot;

        // Charge
        if (text.Contains("declare a charge") || text.Contains("eligible to charge") ||
            text.Contains("overwatch") || text.Contains("charge roll") ||
            text.Contains("charge move"))
            phases |= GamePhase.Charge;

        // Fight
        if (text.Contains("selected to fight") || text.Contains("pile in") ||
            text.Contains("consolidat") || text.Contains("weapon skill") ||
            text.Contains("melee attack"))
            phases |= GamePhase.Fight;

        // Generic attack / save patterns → both shooting and fighting
        if (phases == GamePhase.None)
        {
            if (text.Contains("hit roll") || text.Contains("wound roll") ||
                text.Contains("damage roll") || text.Contains("saving throw") ||
                text.Contains("an attack") || text.Contains("attacks"))
                phases |= GamePhase.Shoot | GamePhase.Fight;
        }

        return phases == GamePhase.None ? GamePhase.All : phases;
    }

    [GeneratedRegex(@"(\d+\+)\s*invulnerable", RegexOptions.IgnoreCase)]
    private static partial Regex InvulnRegex();

    [GeneratedRegex(@"would lose a wound.*?(\d\+).*?wound is not lost", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private static partial Regex FnpWoundRegex();

    [GeneratedRegex(@"feel no pain (\d\+)", RegexOptions.IgnoreCase)]
    private static partial Regex FnpExplicitRegex();

    [GeneratedRegex(@"subtract\s+(\d+)\s+from\s+(?:that\s+attack'?s?\s+|the\s+)?(Hit|Wound)\s+roll", RegexOptions.IgnoreCase)]
    private static partial Regex DefensiveModifierRegex();

    [GeneratedRegex(@"ends?\s+a\s+Charge\s+move", RegexOptions.IgnoreCase)]
    private static partial Regex ChargeEffectRegex();

    private static string BuildChargeSummary(string desc)
    {
        // Try to extract mortal wound info
        var mw = Regex.Matches(desc, @"on\s+a?\s*(\d+(?:-\d+)?)\s*[,:]\s*(?:that\s+unit\s+)?suffers?\s+(D?\d+(?:\+\d+)?)\s+mortal\s+wounds?", RegexOptions.IgnoreCase);
        if (mw.Count > 0)
        {
            var parts = mw.Select(m => $"{m.Groups[2].Value} MW ({m.Groups[1].Value})");
            return string.Join(", ", parts);
        }
        // Fallback: generic mortal wound mention
        var simple = Regex.Match(desc, @"suffers?\s+(D?\d+(?:\+\d+)?)\s+mortal\s+wounds?", RegexOptions.IgnoreCase);
        if (simple.Success)
            return $"{simple.Groups[1].Value} MW";
        return "On Charge";
    }

    private static Match MatchFnp(string desc)
    {
        var m = FnpExplicitRegex().Match(desc);
        return m.Success ? m : FnpWoundRegex().Match(desc);
    }

    private static bool IsPreGameProfileName(string name) =>
        name.StartsWith("Scout", StringComparison.OrdinalIgnoreCase) ||
        name.StartsWith("Infiltrator", StringComparison.OrdinalIgnoreCase) ||
        name.StartsWith("Deep Strike", StringComparison.OrdinalIgnoreCase);

    private static string BetterValue(string current, string candidate)
    {
        if (string.IsNullOrEmpty(current)) return candidate;
        if (string.IsNullOrEmpty(candidate)) return current;
        return string.Compare(current, candidate, StringComparison.Ordinal) <= 0 ? current : candidate;
    }
}
