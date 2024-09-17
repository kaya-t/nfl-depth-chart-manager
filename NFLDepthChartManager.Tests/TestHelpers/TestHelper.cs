using Newtonsoft.Json;
using NFLDepthChartManager.Dtos;
using NFLDepthChartManager.Interfaces;
using Moq;

namespace NFLDepthChartManager.Tests.TestHelpers;

public static class TestHelper
{
    public static Dictionary<string, Dictionary<string, List<Player>>>? LoadTeamData()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var path = Path.Combine(baseDirectory, "..", "..", "..", "TestData", "test_teams.json");

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"File not found: {path}");
        }

        var data = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<Player>>>>(data);
    }

    public static void MockDepthChartRepository(Mock<IDepthChartRepository> mockDepthChartRepository, string teamName, Dictionary<string, List<Player>> playersList)
    {
        mockDepthChartRepository.Setup(repository => repository.GetTeam(It.IsAny<string>()))
            .Returns(new Team(teamName) { DepthChart = playersList });

        mockDepthChartRepository.Setup(repository => repository.GetFullDepthChart(It.IsAny<string>()))
            .Returns(playersList);
    }
}