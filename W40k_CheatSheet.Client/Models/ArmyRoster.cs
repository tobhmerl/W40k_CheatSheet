namespace W40k_CheatSheet.Client.Models;

public class ArmyRoster
{
    public string Name { get; set; } = "";
    public string Faction { get; set; } = "";
    public string Detachment { get; set; } = "";
    public string DetachmentRule { get; set; } = "";
    public int TotalPoints { get; set; }
    public int PointLimit { get; set; }
    public List<UnitEntry> Units { get; set; } = [];
}

public class UnitEntry
{
    public string Name { get; set; } = "";
    public int Points { get; set; }
    public List<string> Keywords { get; set; } = [];
    public List<StatLine> StatLines { get; set; } = [];
    public List<WeaponProfile> RangedWeapons { get; set; } = [];
    public List<WeaponProfile> MeleeWeapons { get; set; } = [];
    public List<AbilityEntry> Abilities { get; set; } = [];
    public string InvulnerableSave { get; set; } = "";
    public string InvulnerableSaveAura { get; set; } = "";
    public string FeelNoPain { get; set; } = "";
    public string FeelNoPainAura { get; set; } = "";
    public bool IsWarlord { get; set; }
    public bool IsLeader { get; set; }
    public List<string> Enhancements { get; set; } = [];
    public List<UnitEntry> AttachedLeaders { get; set; } = [];
    public int ModelCount { get; set; } = 1;
    public List<DefensiveModifier> DefensiveModifiers { get; set; } = [];
}

public class StatLine
{
    public string Name { get; set; } = "";
    public string M { get; set; } = "";
    public string T { get; set; } = "";
    public string SV { get; set; } = "";
    public string W { get; set; } = "";
    public string LD { get; set; } = "";
    public string OC { get; set; } = "";
}

public class WeaponProfile
{
    public string Name { get; set; } = "";
    public string Range { get; set; } = "";
    public string A { get; set; } = "";
    public string Skill { get; set; } = "";
    public string S { get; set; } = "";
    public string AP { get; set; } = "";
    public string D { get; set; } = "";
    public string Keywords { get; set; } = "";
    public int ModelsEquipped { get; set; } = 1;
}

[Flags]
public enum GamePhase
{
    None = 0,
    Command = 1,
    Move = 2,
    Shoot = 4,
    Charge = 8,
    Fight = 16,
    All = Command | Move | Shoot | Charge | Fight
}

public class AbilityEntry
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public GamePhase Phases { get; set; } = GamePhase.All;
}

public enum StratagemTurn
{
    Green,
    Blue,
    Red,
    BlueRed,
    RedBlue
}

public class Stratagem
{
    public string Name { get; set; } = "";
    public string Cost { get; set; } = "";
    public string Category { get; set; } = "";
    public string When { get; set; } = "";
    public string Target { get; set; } = "";
    public string Effect { get; set; } = "";
    public string Restriction { get; set; } = "";
    public string FullWhen { get; set; } = "";
    public string FullTarget { get; set; } = "";
    public string FullEffect { get; set; } = "";
    public string FullRestriction { get; set; } = "";
    public GamePhase Phases { get; set; } = GamePhase.All;
    public GamePhase? MyTurnPhases { get; set; }
    public GamePhase? EnemyTurnPhases { get; set; }
    public List<string> RequiredKeywords { get; set; } = [];
    public string Detachment { get; set; } = "";
    public StratagemTurn TurnColor { get; set; } = StratagemTurn.Green;
}

public class RuleEntry
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
}

public class DetachmentEffectDefinition
{
    public string Detachment { get; set; } = "";
    public List<DetachmentEffect> Effects { get; set; } = [];
}

public class DetachmentEffect
{
    /// <summary>bs, ws, attacks, strength, ap, damage, toughness, save</summary>
    public string Stat { get; set; } = "";
    /// <summary>+1, -1, +2, etc.</summary>
    public int Modifier { get; set; }
    /// <summary>melee, ranged, all</summary>
    public string WeaponType { get; set; } = "all";
    /// <summary>Condition key: always, has_leader_character, charged, battle_shocked_and_charged, etc.</summary>
    public string Condition { get; set; } = "always";
    /// <summary>If true, this effect is reflected directly in stats (hides detachment rule text from card)</summary>
    public bool ReflectedInStats { get; set; }
    /// <summary>Short display label for the buff source tooltip</summary>
    public string SourceLabel { get; set; } = "";
}

/// <summary>A parsed defensive modifier from an ability (e.g. "subtract 1 from the Wound roll").</summary>
public class DefensiveModifier
{
    /// <summary>"hit" or "wound"</summary>
    public string Roll { get; set; } = "";
    /// <summary>Modifier value, e.g. -1</summary>
    public int Value { get; set; }
    /// <summary>"melee", "ranged", or "all"</summary>
    public string AttackType { get; set; } = "all";
    /// <summary>Optional condition text for display, e.g. "S &gt; T"</summary>
    public string Condition { get; set; } = "";
    /// <summary>Source ability name</summary>
    public string Source { get; set; } = "";
}
