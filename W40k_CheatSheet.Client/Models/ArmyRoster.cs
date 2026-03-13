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
    public string FeelNoPain { get; set; } = "";
    public string FeelNoPainAura { get; set; } = "";
    public bool IsWarlord { get; set; }
    public bool IsLeader { get; set; }
    public List<string> Enhancements { get; set; } = [];
    public List<UnitEntry> AttachedLeaders { get; set; } = [];
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

public class Stratagem
{
    public string Name { get; set; } = "";
    public string Cost { get; set; } = "";
    public string Category { get; set; } = "";
    public string When { get; set; } = "";
    public string Target { get; set; } = "";
    public string Effect { get; set; } = "";
    public string Restriction { get; set; } = "";
    public GamePhase Phases { get; set; } = GamePhase.All;
    public List<string> RequiredKeywords { get; set; } = [];
}

public class RuleEntry
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
}
