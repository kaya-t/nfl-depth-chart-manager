using NFLDepthChartManager.Interfaces;

namespace NFLDepthChartManager.Configuration;

public enum FootballCategories
{
    Offense,
    Defense,
    SpecialTeams,
    Reserves,
    Unknown
}

public class NFLConfig : ISportConfig
{
    private static readonly Dictionary<string, FootballCategories> CategoriesMap = new()
    {
        { "QB", FootballCategories.Offense },
        { "RB", FootballCategories.Offense },
        { "WR", FootballCategories.Offense },
        { "TE", FootballCategories.Offense },
        { "LT", FootballCategories.Offense },
        { "LG", FootballCategories.Offense },
        { "C", FootballCategories.Offense },
        { "RG", FootballCategories.Offense },
        { "RT", FootballCategories.Offense },
        { "DE", FootballCategories.Defense },
        { "NT", FootballCategories.Defense },
        { "OLB", FootballCategories.Defense },
        { "ILB", FootballCategories.Defense },
        { "CB", FootballCategories.Defense },
        { "SS", FootballCategories.Defense },
        { "FS", FootballCategories.Defense },
        { "RCB", FootballCategories.Defense },
        { "PT", FootballCategories.SpecialTeams },
        { "PK", FootballCategories.SpecialTeams },
        { "LS", FootballCategories.SpecialTeams },
        { "H", FootballCategories.SpecialTeams },
        { "KO", FootballCategories.SpecialTeams },
        { "PR", FootballCategories.SpecialTeams },
        { "KR", FootballCategories.SpecialTeams },
        { "RES", FootballCategories.Reserves },
        { "FUT", FootballCategories.Reserves }
    };

    public bool IsPositionValid(string? position)
    {
        return !string.IsNullOrEmpty(position) && CategoriesMap.ContainsKey(position);
    }

    public string GetCategory(string? position)
    {
        if (string.IsNullOrEmpty(position)) return FootballCategories.Unknown.ToString();
        return CategoriesMap.TryGetValue(position, out var category) ? category.ToString() : FootballCategories.Unknown.ToString();
    }
}