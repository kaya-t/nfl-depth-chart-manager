using NFLDepthChartManager.Dtos;
using NFLDepthChartManager.Interfaces;

namespace NFLDepthChartManager.Services;

public class PlayerTeamManagementService(IDepthChartRepository depthChartRepository) : IPlayerTeamManagementService
{
    public void AddPlayerToTeamDepthChart(string teamName, string? position, Player player, int? positionDepth)
    {
        var team = GetTeam(teamName);

        if (position != null && !team.DepthChart.ContainsKey(position))
        {
            team.DepthChart[position] = new List<Player>();
        }

        if (position == null) return;
        var players = team.DepthChart[position];

        if (positionDepth.HasValue && positionDepth.Value <= players.Count)
        {
            players.Insert(positionDepth.Value, player);
        }
        else
        {
            players.Add(player);
        }
    }

    public Player? RemovePlayerFromTeamDepthChart(string teamName, string? position, Player player)
    {
        var team = GetTeam(teamName);

        if (!team.DepthChart.TryGetValue(position?.ToUpper() ?? throw new InvalidOperationException(), out var players))
            return null;

        var playerToRemove = players.FirstOrDefault(p => string.Equals(p.Name.Trim(), player.Name.Trim(), StringComparison.OrdinalIgnoreCase));

        if (playerToRemove == null)
            return null;

        players.Remove(playerToRemove);
        return playerToRemove;
    }

    public IEnumerable<Player> GetBackupsForTeam(string teamName, string? position, Player player)
    {
        var team = GetTeam(teamName);

        if (!team.DepthChart.TryGetValue(position?.ToUpper() ?? throw new InvalidOperationException(), out var players))
            return Array.Empty<Player>();

        var playerInChart = players.FirstOrDefault(p => p.Name.Equals(player.Name, StringComparison.OrdinalIgnoreCase));
        if (playerInChart == null)
            return Array.Empty<Player>();

        var index = players.IndexOf(playerInChart);
        return index == players.Count - 1 ? Array.Empty<Player>() : players.Skip(index + 1);
    }

    public void AddTeam(string teamName)
    {
        if (!depthChartRepository.TeamExists(teamName))
        {
            var newTeam = new Team(teamName);
            depthChartRepository.AddTeam(newTeam);
        }
    }

    public IEnumerable<Team> GetAllTeams() => depthChartRepository.GetAllTeams();

    public Dictionary<string, List<Player>> GetFullTeamDepthChart(string teamName) => GetTeam(teamName).DepthChart;

    public bool IsPlayerNumberExists(string teamName, int number) => depthChartRepository
        .GetTeam(teamName)
        .DepthChart
        .Any(x => x.Value.Any(player => player.Number == number));

    private Team GetTeam(string teamName)
    {
        if (!depthChartRepository.TeamExists(teamName))
        {
            var newTeam = new Team(teamName);
            depthChartRepository.AddTeam(newTeam);
        }

        return depthChartRepository.GetTeam(teamName);
    }
}