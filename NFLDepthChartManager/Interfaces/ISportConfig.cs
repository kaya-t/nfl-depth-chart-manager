namespace NFLDepthChartManager.Interfaces;

public interface ISportConfig
{
    bool IsPositionValid(string? position);
        
    string GetCategory(string? position);
}