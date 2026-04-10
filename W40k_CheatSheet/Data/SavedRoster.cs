namespace W40k_CheatSheet.Data;

public class SavedRoster
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Faction { get; set; } = "";
    public string Detachment { get; set; } = "";
    public int Points { get; set; }

    /// <summary>Serialized SavedArmyData JSON (rawJson + state + configs).</summary>
    public string DataJson { get; set; } = "";

    public DateTime LastModified { get; set; }

    public AppUser User { get; set; } = null!;
}
