using System.Text.Json.Serialization;

namespace W40k_CheatSheet.Client.Models;

public class RosterRoot
{
    [JsonPropertyName("roster")]
    public RosterData Roster { get; set; } = new();
}

public class RosterData
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("costs")]
    public List<Cost> Costs { get; set; } = [];

    [JsonPropertyName("costLimits")]
    public List<Cost> CostLimits { get; set; } = [];

    [JsonPropertyName("forces")]
    public List<Force> Forces { get; set; } = [];

    [JsonPropertyName("gameSystemName")]
    public string GameSystemName { get; set; } = "";
}

public class Cost
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("typeId")]
    public string TypeId { get; set; } = "";

    [JsonPropertyName("value")]
    public double Value { get; set; }
}

public class Force
{
    [JsonPropertyName("selections")]
    public List<Selection> Selections { get; set; } = [];

    [JsonPropertyName("categories")]
    public List<Category> Categories { get; set; } = [];

    [JsonPropertyName("rules")]
    public List<Rule> Rules { get; set; } = [];

    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("catalogueName")]
    public string CatalogueName { get; set; } = "";
}

public class Selection
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("entryId")]
    public string EntryId { get; set; } = "";

    [JsonPropertyName("number")]
    public int Number { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = "";

    [JsonPropertyName("from")]
    public string From { get; set; } = "";

    [JsonPropertyName("group")]
    public string Group { get; set; } = "";

    [JsonPropertyName("costs")]
    public List<Cost> Costs { get; set; } = [];

    [JsonPropertyName("categories")]
    public List<Category> Categories { get; set; } = [];

    [JsonPropertyName("profiles")]
    public List<Profile> Profiles { get; set; } = [];

    [JsonPropertyName("selections")]
    public List<Selection> Selections { get; set; } = [];

    [JsonPropertyName("rules")]
    public List<Rule> Rules { get; set; } = [];
}

public class Category
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("entryId")]
    public string EntryId { get; set; } = "";

    [JsonPropertyName("primary")]
    public bool Primary { get; set; }
}

public class Profile
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("hidden")]
    public bool Hidden { get; set; }

    [JsonPropertyName("typeId")]
    public string TypeId { get; set; } = "";

    [JsonPropertyName("typeName")]
    public string TypeName { get; set; } = "";

    [JsonPropertyName("characteristics")]
    public List<Characteristic> Characteristics { get; set; } = [];
}

public class Characteristic
{
    [JsonPropertyName("$text")]
    public string Text { get; set; } = "";

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("typeId")]
    public string TypeId { get; set; } = "";
}

public class Rule
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("description")]
    public string Description { get; set; } = "";

    [JsonPropertyName("hidden")]
    public bool Hidden { get; set; }
}
