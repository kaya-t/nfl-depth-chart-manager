using Moq;
using NFLDepthChartManager.Dtos;
using NFLDepthChartManager.Interfaces;
using NFLDepthChartManager.Services;
using NFLDepthChartManager.Tests.TestHelpers;

namespace NFLDepthChartManager.Tests.Services;

public class PlayerTeamManagementServiceTests
{
    private readonly Mock<IDepthChartRepository> _mockRepository;
    private readonly PlayerTeamManagementService _teamManagementService;
    private Dictionary<string, List<Player>> _teamData;
    private readonly string _teamName = "Tampa Bay Buccaneers";

    public PlayerTeamManagementServiceTests()
    {
        _mockRepository = new Mock<IDepthChartRepository>();
        _teamData = TestHelper.LoadTeamData()?[_teamName];

        TestHelper.MockDepthChartRepository(_mockRepository, _teamName, _teamData);

        _teamManagementService = new PlayerTeamManagementService(_mockRepository.Object);
    }

    [Fact]
    public void Add_New_Player_With_Depth_Should_Be_Added_Successfully()
    {
        // Arrange
        var newPlayer = new Player(99, "Ninety Nine", "QB", "Offense");

        // Act
        var exceptionResult = Record.Exception(() => _teamManagementService
            .AddPlayerToTeamDepthChart(_teamName, "QB", newPlayer, 100));

        // Assert
        Assert.Null(exceptionResult);
        Assert.Equal(3, _teamData["QB"].Count);
    }

    [Fact]
    public void Remove_Player_Using_Invalid_Position_Should_Return_Null()
    {
        // Arrange
        var nonExistentPlayer = new Player(999, "NotExisting", "NonExistingPosition", "Offense");

        // Act
        var removedPlayer = _teamManagementService.RemovePlayerFromTeamDepthChart(_teamName, "NonExistingPosition", nonExistentPlayer);

        // Assert
        Assert.Null(removedPlayer);
    }

    [Fact]
    public void Get_Backups_For_Invalid_Position_Should_Return_Empty_List()
    {
        // Arrange
        var nonExistentPlayer = new Player(999, "NotExisting", "InvalidPosition", "Offense");

        // Act
        var backups = _teamManagementService.GetBackupsForTeam(_teamName, "InvalidPosition", nonExistentPlayer);

        // Assert
        Assert.Empty(backups);
    }

    [Fact]
    public void Adding_New_Team_Should_Add_Team_Successfully()
    {
        // Arrange
        var newTeamName = "Miami Dolphins";
        _mockRepository.Setup(repo => repo.TeamExists(newTeamName)).Returns(false);

        // Act
        _teamManagementService.AddTeam(newTeamName);

        // Assert
        _mockRepository.Verify(repo => repo.AddTeam(It.IsAny<Team>()), Times.Once);
    }

    [Fact]
    public void Select_Team_When_Exists_Should_Return_Team_Data()
    {
        // Arrange
        string selectedTeamName = "Eagles";
        _teamData = TestHelper.LoadTeamData()?[selectedTeamName];
        TestHelper.MockDepthChartRepository(_mockRepository, selectedTeamName, _teamData);

        // Act
        var fetchedTeam = _teamManagementService.GetFullTeamDepthChart(selectedTeamName);

        // Assert
        Assert.NotNull(fetchedTeam);
        Assert.Equal("Sixteen", fetchedTeam["LWR"].First().Name);
    }

    [Fact]
    public void Add_Player_To_Selected_Team_Should_Be_Added_Properly()
    {
        // Arrange
        string selectedTeamName = "Eagles";
        _teamData = TestHelper.LoadTeamData()?[selectedTeamName];
        TestHelper.MockDepthChartRepository(_mockRepository, selectedTeamName, _teamData);

        var newPlayer = new Player(99, "Ninety Nine", "RB", "Offense");

        // Act
        _teamManagementService.AddPlayerToTeamDepthChart(selectedTeamName, "RB", newPlayer, null);

        // Assert
        Assert.Equal("Ninety Nine", _teamData["RB"].Last().Name);
    }

    [Fact]
    public void Remove_Player_From_Team_Should_Be_Removed_Successfully()
    {
        // Arrange
        string selectedTeamName = "Eagles";
        _teamData = TestHelper.LoadTeamData()?[selectedTeamName];
        TestHelper.MockDepthChartRepository(_mockRepository, selectedTeamName, _teamData);

        var playerToRemove = _teamData["QB"].First();

        // Act
        var removedPlayer = _teamManagementService.RemovePlayerFromTeamDepthChart(selectedTeamName, "QB", playerToRemove);

        // Assert
        Assert.NotNull(removedPlayer);
        Assert.Equal("One", removedPlayer?.Name);
    }

    [Fact]
    public void Add_Player_At_Specific_Depth_Should_Place_At_Correct_Location()
    {
        // Arrange
        var newPlayer = new Player(9, "Nine", "QB", "Offense");

        // Act
        _teamManagementService.AddPlayerToTeamDepthChart(_teamName, "QB", newPlayer, 1);

        // Assert
        _mockRepository.Verify(repo => repo.GetTeam(_teamName), Times.Once);
        Assert.Equal("Nine", _teamData["QB"][1].Name);
    }

    [Fact]
    public void Add_Player_Without_Depth_Should_Place_At_End()
    {
        // Arrange
        var newPlayer = new Player(15, "Fifteen", "RB", "Offense");

        // Act
        _teamManagementService.AddPlayerToTeamDepthChart(_teamName, "RB", newPlayer, null);

        // Assert
        _mockRepository.Verify(repo => repo.GetTeam(_teamName), Times.Once);
        Assert.Equal("Fifteen", _teamData["RB"].Last().Name);
    }

    [Fact]
    public void Add_Player_At_Existing_Depth_Should_Shift_Other_Players_Down()
    {
        // Arrange
        var newPlayer = new Player(17, "Seventeen", "QB", "Offense");

        // Act
        _teamManagementService.AddPlayerToTeamDepthChart(_teamName, "QB", newPlayer, 0);

        // Assert
        _mockRepository.Verify(repo => repo.GetTeam(_teamName), Times.Once);
        Assert.Equal("Seventeen", _teamData["QB"][0].Name);
        Assert.Equal("Twelve", _teamData["QB"][1].Name);
    }

    [Fact]
    public void Remove_Player_From_Team_Should_Return_Removed_Player()
    {
        // Act
        var playerToRemove = _teamData["QB"].First();

        // Arrange
        var removedPlayer = _teamManagementService.RemovePlayerFromTeamDepthChart(_teamName, "QB", playerToRemove);

        // Assert
        Assert.NotNull(removedPlayer);
        Assert.Equal("Twelve", removedPlayer?.Name);
        _mockRepository.Verify(repo => repo.GetTeam(_teamName), Times.Once);
    }

    [Fact]
    public void Try_Remove_NonExistent_Player_Should_Return_Null()
    {
        // Arrange
        var nonExistentPlayer = new Player(999, "NonExistent", "QB", "Offense");

        // Act
        var removedPlayer = _teamManagementService.RemovePlayerFromTeamDepthChart(_teamName, "QB", nonExistentPlayer);

        // Assert
        Assert.Null(removedPlayer);
        _mockRepository.Verify(repo => repo.GetTeam(_teamName), Times.Once);
    }

    [Fact]
    public void Get_Backups_For_Valid_Player_Should_Return_Backups()
    {
        // Arrange
        var validPlayer = _teamData["RB"].First();

        // Act
        var backupsList = _teamManagementService.GetBackupsForTeam(_teamName, "RB", validPlayer).ToList();

        // Assert
        Assert.NotEmpty(backupsList);
        Assert.Equal("Twenty One", backupsList.First().Name);
    }

    [Fact]
    public void Get_Backups_When_None_Exist_Should_Return_Empty_List()
    {
        // Arrange
        var lastPlayer = _teamData["PT"].Last();

        // Act
        var backupsResult = _teamManagementService.GetBackupsForTeam(_teamName, "PT", lastPlayer);

        // Assert
        Assert.Empty(backupsResult);
    }

    [Fact]
    public void Get_Backups_For_Invalid_Player_Should_Return_Empty_List()
    {
        // Arrange
        var nonExistentPlayer = new Player(100, "NotAPlayer", "LWR", "Offense");

        // Act
        var backups = _teamManagementService.GetBackupsForTeam(_teamName, "LWR", nonExistentPlayer);

        // Assert
        Assert.Empty(backups);
    }
}