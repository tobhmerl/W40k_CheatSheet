namespace W40k_CheatSheet.Client.Models;

/// <summary>Per-ability setup configuration (turn / phase visibility + apply-to-stats opt-in).</summary>
public sealed class AbilitySetupEntry
{
    public bool MyTurn    { get; set; }
    public bool EnemyTurn { get; set; }
    public bool Command   { get; set; }
    public bool Move      { get; set; }
    public bool Shoot     { get; set; }
    public bool Charge    { get; set; }
    public bool Fight     { get; set; }
    public bool EnemyCommand { get; set; }
    public bool EnemyMove    { get; set; }
    public bool EnemyShoot   { get; set; }
    public bool EnemyCharge  { get; set; }
    public bool EnemyFight   { get; set; }
    public bool MyCommand  { get; set; }
    public bool MyMove     { get; set; }
    public bool MyShooting { get; set; }
    public bool MyCharge   { get; set; }
    public bool MyFight    { get; set; }
    public bool ApplyToStats { get; set; }
}

/// <summary>Per-stratagem setup configuration.</summary>
public sealed class StratagemSetupEntry
{
    public bool MyTurn    { get; set; }
    public bool EnemyTurn { get; set; }
    public bool Command   { get; set; }
    public bool Move      { get; set; }
    public bool Shoot     { get; set; }
    public bool Charge    { get; set; }
    public bool Fight     { get; set; }
    public bool EnemyCommand { get; set; }
    public bool EnemyMove    { get; set; }
    public bool EnemyShoot   { get; set; }
    public bool EnemyCharge  { get; set; }
    public bool EnemyFight   { get; set; }
    public bool MyCommand  { get; set; }
    public bool MyMove     { get; set; }
    public bool MyShooting { get; set; }
    public bool MyCharge   { get; set; }
    public bool MyFight    { get; set; }
    public bool Disabled  { get; set; }
}

/// <summary>Configuration for the detachment rule itself (turn / phase visibility for the reminder text).</summary>
public sealed class DetachmentSetupEntry
{
    public bool MyTurn    { get; set; }
    public bool EnemyTurn { get; set; }
    public bool Command   { get; set; }
    public bool Move      { get; set; }
    public bool Shoot     { get; set; }
    public bool Charge    { get; set; }
    public bool Fight     { get; set; }
    public bool EnemyCommand { get; set; }
    public bool EnemyMove    { get; set; }
    public bool EnemyShoot   { get; set; }
    public bool EnemyCharge  { get; set; }
    public bool EnemyFight   { get; set; }
    public bool MyCommand  { get; set; }
    public bool MyMove     { get; set; }
    public bool MyShooting { get; set; }
    public bool MyCharge   { get; set; }
    public bool MyFight    { get; set; }
    public bool Reminder  { get; set; }
}

/// <summary>Per detachment-rule effect (one row per entry in detachment_effects.json). Default: nothing applied.</summary>
public sealed class DetachmentEffectSetupEntry
{
    /// <summary>Show as reminder in play view.</summary>
    public bool Enabled      { get; set; }
    /// <summary>Actually modify weapon/unit stats.</summary>
    public bool ApplyToStats { get; set; }
}

/// <summary>Per faction/army rule (e.g. Reanimation Protocols). Default: nothing applied.</summary>
public sealed class ArmyRuleSetupEntry
{
    public bool Enabled      { get; set; }
    public bool ApplyToStats { get; set; }
    public bool Reminder     { get; set; }
}

/// <summary>Persisted ordering / leader-attachment state for the current army.</summary>
public sealed class RosterState
{
    public List<string> UnitOrder { get; set; } = [];
    public List<AttachmentInfo> Attachments { get; set; } = [];
}

public sealed class AttachmentInfo
{
    public string Leader { get; set; } = "";
    public string Host { get; set; } = "";
}

/// <summary>Bundle of all setup configs for a single saved army (used by Save/Load).</summary>
public sealed class SavedArmyData
{
    public string RawJson { get; set; } = "";
    public RosterState? State { get; set; }
    public Dictionary<string, AbilitySetupEntry> AbilityConfigs { get; set; } = new();
    public Dictionary<string, StratagemSetupEntry> StratagemConfigs { get; set; } = new();
    public DetachmentSetupEntry? DetachmentConfig { get; set; }
    public Dictionary<string, DetachmentEffectSetupEntry> DetachmentEffectConfigs { get; set; } = new();
    public Dictionary<string, ArmyRuleSetupEntry> ArmyRuleConfigs { get; set; } = new();
}

public sealed class SavedArmyMeta
{
    public string Name { get; set; } = "";
    public string Faction { get; set; } = "";
    public string Detachment { get; set; } = "";
    public int Points { get; set; }
}
