using NFLDepthChartManager.Dtos;

namespace NFLDepthChartManager.Interfaces;

public interface IPlayerTeamManagementService
{
    void AddPlayerToTeamDepthChart(string teamName, string? position, Player player, int? positionDepth);
    Player? RemovePlayerFromTeamDepthChart(string teamName, string? position, Player player);
    Dictionary<string, List<Player>> GetFullTeamDepthChart(string teamName);
    bool IsPlayerNumberExists(string teamName, int number);
    IEnumerable<Player> GetBackupsForTeam(string teamName, string? position, Player player);
    void AddTeam(string teamName);
    IEnumerable<Team> GetAllTeams();
}