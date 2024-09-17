namespace NFLDepthChartManager.Dtos;

public class Player(int number, string name, string position, string category)
{
    public int Number { get;} = number;
    public string Name { get; } = name;
    public string Position { get; } = position;
    public string Category { get; } = category;
}