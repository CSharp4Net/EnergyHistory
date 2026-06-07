using CS4N.EnergyHistory.Contracts.Models.ElectricMeter;
using CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data;

namespace CS4N.EnergyHistory.WebApp.ViewModels.ElectricMeter
{
  public sealed class DataEditView
  {
    public ElectricMeterDefinition? Definition { get; set; }

    public ElectricMeterDataObject? Data { get; set; }
  }
}