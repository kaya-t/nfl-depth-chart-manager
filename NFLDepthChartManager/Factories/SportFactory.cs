using NFLDepthChartManager.Configuration;
using NFLDepthChartManager.Interfaces;

namespace NFLDepthChartManager.Factories;

public class SportFactory : ISportFactory
{
    public ISportConfig GetSportConfig()
    {
        return new NFLConfig();
    }
}