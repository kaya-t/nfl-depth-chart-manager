using NFLDepthChartManager.Dtos;

namespace NFLDepthChartManager.Interfaces;

public interface IDepthChartRepository
{
    void AddTeam(Team team);
    Team GetTeam(string teamName);
    bool TeamExists(string teamName);
    Dictionary<string, List<Player>> GetFullDepthChart(string teamName);
    IEnumerable<Team> GetAllTeams();
}