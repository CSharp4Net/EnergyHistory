using CS4N.EnergyHistory.Contracts.Models.ElectricMeter;
using CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data;

namespace CS4N.EnergyHistory.WebApp.ViewModels.ElectricMeter
{
  public sealed class DataView
  {
    //public double GeneratedElectricityAmount { get; set; }
    //public decimal GeneratedElectricityValue { get; set; }
    //public decimal FedInElectricityValue { get; set; }

    public List<ChartDataEntry> ChartData { get; set; } = [];

    public List<DataObject> Datas { get; internal set; } = [];
  }
}