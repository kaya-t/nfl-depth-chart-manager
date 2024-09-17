namespace NFLDepthChartManager.Interfaces;

public interface IOrchestratorService
{
    void Start();

    void AddPlayer();
        
    void RemovePlayer();

    void ShowBackups();
        
    void ShowDepthChart();

    void PickTeam();

}