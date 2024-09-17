using Moq;
using NFLDepthChartManager.Interfaces;
using NFLDepthChartManager.Services;
using NFLDepthChartManager.Dtos;
using NFLDepthChartManager.Tests.TestHelpers;

namespace NFLDepthChartManager.Tests.Services;

public class IOServiceTests
{
    private readonly IOService _ioService;
    private readonly Mock<IPlayerTeamManagementService> _mockPlayerService;

    public IOServiceTests()
    {
        _mockPlayerService = new Mock<IPlayerTeamManagementService>();
        _ioService = new IOService(_mockPlayerService.Object);
    }

    [Fact]
    public void Should_Save_Depth_Chart_To_File()
    {
        // Arrange
        var teams = LoadTeamsFromFile();
        _mockPlayerService.Setup(service => service.GetAllTeams()).Returns(teams);

        // Act
        _ioService.SaveDepthCharts();

        // Assert
        _mockPlayerService.Verify(service => service.GetAllTeams(), Times.Once);
    }

    private List<Team> LoadTeamsFromFile()
    {
        var data = TestHelper.LoadTeamData();

        return data.Select(team 
                => new Team(team.Key) { DepthChart = team.Value })
            .ToList();
    }
}