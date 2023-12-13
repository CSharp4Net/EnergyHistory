using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.Contracts.Models.Data
{
  public sealed class StationDataYear
  {
    public int Number { get; set; }
    public double CollectedTotal { get; set; }
    public bool ManualInput { get; set; }

    public List<StationDataMonth> Months { get; set; } = [];
  }
}