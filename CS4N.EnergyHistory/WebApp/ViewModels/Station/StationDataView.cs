using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.WebApp.ViewModels.Station
{
  public sealed class StationDataView
  {
    public double GeneratedElectricityAmount { get; set; }
    public decimal GeneratedElectricityValue { get; set; }
    public decimal FedInElectricityValue { get; set; }

    public List<ChartDataEntry> ChartData { get; set; } = [];

    public StationDefinition? StationDefinition { get; internal set; }
  }
}