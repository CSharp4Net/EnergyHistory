using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.WebApp.ViewModels.Station
{
  public sealed class StationDataView
  {
    public double StationCollectedTotal { get; set; }

    public List<ChartDataEntry> ChartData { get; set; } = [];

    public StationDefinition? StationDefinition { get; internal set; }
  }
}