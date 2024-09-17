using NFLDepthChartManager.Dtos;
using NFLDepthChartManager.Interfaces;

namespace NFLDepthChartManager.Services;

public class OrchestratorService(
    IPlayerTeamManagementService playerTeamManagementService,
    IIOService ioService,
    ISportFactory sportFactory) : IOrchestratorService
{
    private string _selectedTeamName = "";
    private ISportConfig? _sportConfig;

    private readonly string _playerNameQuestion = "Player name?";
    private readonly string _playerPositionQuestion = "Player position?";

    public void Start()
    {
        Console.WriteLine(".\u2740。• *\u208a\u00b0。 \u2740\u00b0。 NFL Depth Charts .\u2740。• *\u208a\u00b0。 \u2740\u00b0。 ");

        _sportConfig = sportFactory.GetSportConfig();
        ioService.LoadDepthCharts();
        PickTeam();

        while (true)
        {
            ShowMenu();

            var input = Console.ReadLine()?.Trim();
            if (!ProcessMenuSelection(input))
            {
                Console.WriteLine("Choose a valid option. Try again.");
            }
        }
    }

    private static void ShowMenu()
    {
        Console.WriteLine("\nSelect option:");
        Console.WriteLine("[1] Add player to team");
        Console.WriteLine("[2] Remove player from team");
        Console.WriteLine("[3] Show player's backups");
        Console.WriteLine("[4] Show team/s depth chart");
        Console.WriteLine("[5] Save chart as new file");
        Console.WriteLine("[6] Save & exit");
    }

    private bool ProcessMenuSelection(string? option)
    {
        return option switch
        {
            "1" => Execute(AddPlayer),
            "2" => Execute(RemovePlayer),
            "3" => Execute(ShowBackups),
            "4" => Execute(ShowDepthChart),
            "5" => Execute(Save),
            "6" => Execute(SaveAndExit),
            _ => false
        };
    }

    private static bool Execute(Action action)
    {
        action.Invoke();
        return true;
    }

    public void PickTeam()
    {
        Console.WriteLine("Enter existing or register new team (press enter for default):");
        var teamName = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(teamName))
        {
            _selectedTeamName = playerTeamManagementService.GetAllTeams().First().Name;
            Console.WriteLine($"Default team selected: {_selectedTeamName}");
        }
        else
        {
            var existingTeam = playerTeamManagementService.GetAllTeams()
                .FirstOrDefault(t => t.Name.Equals(teamName, StringComparison.OrdinalIgnoreCase));

            if (existingTeam != null)
            {
                _selectedTeamName = existingTeam.Name;
                Console.WriteLine($"Team selected: {_selectedTeamName}");
            }
            else
            {
                playerTeamManagementService.AddTeam(teamName);
                _selectedTeamName = teamName;
                Console.WriteLine($"New team created: {teamName}");
            }
        }
    }

    public void AddPlayer()
    {
        var name = GetNonEmptyInput(_playerNameQuestion);
        if (string.IsNullOrWhiteSpace(name)) return;

        var number = GetValidPlayerNumber();
        var position = GetValidPosition();
        var depth = GetPositionDepth();

        var category = _sportConfig?.GetCategory(position);
        var player = new Player(number, name, position, category ?? string.Empty);
        playerTeamManagementService.AddPlayerToTeamDepthChart(_selectedTeamName, position, player, depth);

        Console.WriteLine($"{name} (# {number}) added to {position} ({category}) for {_selectedTeamName}.");
    }

    public void RemovePlayer()
    {
        var position = GetNonEmptyInput(_playerPositionQuestion).ToUpper();
        var playerName = GetNonEmptyInput(_playerNameQuestion);

        if (string.IsNullOrWhiteSpace(playerName) || string.IsNullOrWhiteSpace(position)) return;

        var playerToRemove = new Player(0, playerName, position, string.Empty);
        var removedPlayer = playerTeamManagementService.RemovePlayerFromTeamDepthChart(_selectedTeamName, position, playerToRemove);

        Console.WriteLine(removedPlayer == null
            ? $"Player '{playerName}' - '{position}' not found."
            : $"Removed {removedPlayer.Name} (#{removedPlayer.Number}) from {position} in {_selectedTeamName}.");
    }

    public void ShowBackups()
    {
        var position = GetNonEmptyInput(_playerPositionQuestion);
        var playerName = GetNonEmptyInput(_playerNameQuestion);

        var player = new Player(0, playerName, position, string.Empty);
        var backups = playerTeamManagementService.GetBackupsForTeam(_selectedTeamName, position, player).ToList();

        if (backups.Count != 0)
        {
            Console.WriteLine($"Backups for {playerName} - {position}:");
            foreach (var backup in backups)
            {
                Console.WriteLine($"#{backup.Number} {backup.Name}, ");
            }
        }
        else
        {
            Console.WriteLine($"Backups not found for player {playerName} - {position}.");
        }
    }

    public void ShowDepthChart()
    {
        Console.WriteLine($"NFL Depth Chart for team {_selectedTeamName}:");
        var fullDepthChart = playerTeamManagementService.GetFullTeamDepthChart(_selectedTeamName);

        foreach (var category in fullDepthChart.SelectMany(c => c.Value).GroupBy(p => p.Category))
        {
            Console.WriteLine($"\n{category.Key}:");

            foreach (var position in category.GroupBy(p => p.Position))
            {
                var playerDetails = string.Join(", ", position.Select(p => $"#{p.Number} {p.Name}"));
                Console.WriteLine($"{position.Key}: {playerDetails}");
            }
        }
    }

    private string GetNonEmptyInput(string prompt)
    {
        string input;
        do
        {
            Console.Write(prompt + " ");
            input = Console.ReadLine()?.Trim() ?? string.Empty;
        } while (string.IsNullOrWhiteSpace(input)); // Loop until non-empty input is provided.

        input = System.Text.RegularExpressions.Regex.Replace(input, @"\s+", " "); // Collapse multiple spaces
        return input;
    }

    private int GetValidPlayerNumber()
    {
        int number;

        do
        {
            Console.WriteLine("Player's number?");
        } while (!int.TryParse(Console.ReadLine(), out number) 
                 || playerTeamManagementService.IsPlayerNumberExists(_selectedTeamName, number));

        return number;
    }

    private string GetValidPosition()
    {
        string position;
        do
        {
            Console.WriteLine(_playerPositionQuestion);
            position = Console.ReadLine()?.Trim().ToUpper() ?? string.Empty;
        } while (_sportConfig != null && !_sportConfig.IsPositionValid(position));

        return position;
    }

    private static int? GetPositionDepth()
    {
        Console.WriteLine("Player's depth (0 for starter, blank for default):");
        var input = Console.ReadLine();

        return int.TryParse(input, out var depth) ? depth : null;
    }

    public void Save() => ioService.SaveDepthCharts();

    private void SaveAndExit()
    {
        Save();
        Environment.Exit(0);
    }
}