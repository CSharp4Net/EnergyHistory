using CS4N.EnergyHistory.Contracts.Models.ElectricMeter;

namespace CS4N.EnergyHistory.WebApp.ViewModels.ElectricMeter
{
  public sealed class DataView
  {
    //public double GeneratedElectricityAmount { get; set; }
    //public decimal GeneratedElectricityValue { get; set; }
    //public decimal FedInElectricityValue { get; set; }

    public List<ChartDataEntry> ChartData { get; set; } = [];

    public Definition? Definition { get; internal set; }
  }
}