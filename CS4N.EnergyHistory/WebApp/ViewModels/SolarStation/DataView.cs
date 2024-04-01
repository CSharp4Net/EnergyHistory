using CS4N.EnergyHistory.Contracts.Models.SolarStation;

namespace CS4N.EnergyHistory.WebApp.ViewModels.SolarStation
{
  public sealed class DataView
  {
    public double GeneratedElectricityAmount { get; set; }
    public decimal GeneratedElectricityValue { get; set; }
    public decimal FedInElectricityValue { get; set; }

    public List<ChartDataEntry> ChartData { get; set; } = [];

    public Definition? Definition { get; internal set; }
  }
}