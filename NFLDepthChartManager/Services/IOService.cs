using System.Text.Json;
using NFLDepthChartManager.Dtos;
using NFLDepthChartManager.Interfaces;

namespace NFLDepthChartManager.Services;

public class IOService(IPlayerTeamManagementService playerTeamManagementService) : IIOService
{
    private const string FilePath = "my_team_players.json";
        
    private const string InputFolderName = "nfl_teams";
    private const string OutputFolderName = "output";

    public void LoadDepthCharts()
    {
        var path = Path.Combine(InputFolderName, FilePath);

        if (!File.Exists(path))
        {
            Console.WriteLine("No existing depth chart file found.");
            return;
        }

        var data = File.ReadAllText(path);
        var teams = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, List<Player>>>>(data);

        if (teams == null)
        {
            Console.WriteLine("Failed to load depth chart.");
            return;
        }

        foreach (var (teamName, depthChart) in teams)
        {
            playerTeamManagementService.AddTeam(teamName);
            foreach (var (position, players) in depthChart)
            {
                foreach (var player in players)
                {
                    playerTeamManagementService.AddPlayerToTeamDepthChart(teamName, position, player, null);
                }
            }
        }
    }

    public void SaveDepthCharts()
    {
        var allTeams = playerTeamManagementService.GetAllTeams();
        var teamsData = allTeams.ToDictionary(team => team.Name, team => team.DepthChart);

        var data = JsonSerializer.Serialize(teamsData, new JsonSerializerOptions { WriteIndented = true });

        if (!Directory.Exists(OutputFolderName))
        {
            Directory.CreateDirectory(OutputFolderName);
        }

        var dateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var outputPath = Path.Combine(OutputFolderName, $"my_team_players_{dateTime}.json");

        File.WriteAllText(outputPath, data);

        Console.WriteLine($"Depth chart saved to {outputPath}.");
    }
}