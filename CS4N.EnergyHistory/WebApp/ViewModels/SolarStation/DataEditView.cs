using CS4N.EnergyHistory.Contracts.Models.SolarStation;
using CS4N.EnergyHistory.Contracts.Models.SolarStation.Data;

namespace CS4N.EnergyHistory.WebApp.ViewModels.SolarStation
{
  public sealed class DataEditView
  {
    public Definition? Definition { get; set; }

    public DataSummary? Data { get; set; } 
  }
}