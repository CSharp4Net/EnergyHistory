using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.Contracts.Models.Data
{
  public sealed class StationData (StationDefinition station)
  {
    public StationDefinition Station { get; init; } = station;

    public List<StationDataYear> Years { get; set; } = [];
  }
}