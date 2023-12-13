using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.Contracts.Models.Data
{
  public sealed class StationData
  {
    public string StationId { get; set; }
    public double CollectedTotal { get; set; }
    public bool ManualInput { get; set; }

    public List<StationDataYear> Years { get; set; } = [];
  }
}