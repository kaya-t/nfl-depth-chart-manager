using NFLDepthChartManager.Dtos;
using NFLDepthChartManager.Interfaces;

namespace NFLDepthChartManager.Repositories;

public class InMemoryDepthChartRepository : IDepthChartRepository
{
    private readonly Dictionary<string, Team> _teams = new();

    public void AddTeam(Team team) => _teams.TryAdd(team.Name, team);

    public Team GetTeam(string teamName)
    {
        return _teams.TryGetValue(teamName, out var team)
            ? team
            : throw new InvalidOperationException($"Team {teamName} does not exist.");
    }

    public bool TeamExists(string teamName) => _teams.ContainsKey(teamName);

    public Dictionary<string, List<Player>> GetFullDepthChart(string teamName)
    {
        return _teams.TryGetValue(teamName, out var team)
            ? team.DepthChart
            : throw new InvalidOperationException($"Team {teamName} does not exist.");
    }

    public IEnumerable<Team> GetAllTeams() => _teams.Values;
}