namespace NFLDepthChartManager.Dtos;

public class Team(string name)
{
    public string Name { get; } = name;
    public Dictionary<string, List<Player>> DepthChart { get; init; } = new();
}