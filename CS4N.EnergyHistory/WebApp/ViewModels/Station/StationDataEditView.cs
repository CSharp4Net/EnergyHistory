using CS4N.EnergyHistory.Contracts.Models.Data;
using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.WebApp.ViewModels.Station
{
  public sealed class StationDataEditView
  {
    public StationDefinition? StationDefinition { get; set; }

    public StationData? StationData { get; set; } 
  }
}