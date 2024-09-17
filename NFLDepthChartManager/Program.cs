using Microsoft.Extensions.DependencyInjection;
using NFLDepthChartManager.Configuration;
using NFLDepthChartManager.Factories;
using NFLDepthChartManager.Interfaces;
using NFLDepthChartManager.Repositories;
using NFLDepthChartManager.Services;

namespace NFLDepthChartManager;

internal static class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = ConfigureServices();
        var orchestrator = serviceProvider.GetService<IOrchestratorService>();

        orchestrator?.Start();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IPlayerTeamManagementService, PlayerTeamManagementService>();
        services.AddSingleton<IOrchestratorService, OrchestratorService>();
        services.AddSingleton<IIOService, IOService>();
        services.AddSingleton<ISportConfig, NFLConfig>();
        services.AddSingleton<ISportFactory, SportFactory>();
        services.AddSingleton<IDepthChartRepository, InMemoryDepthChartRepository>();

        return services.BuildServiceProvider();
    }

}