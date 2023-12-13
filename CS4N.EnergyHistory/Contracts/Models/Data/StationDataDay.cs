using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.Contracts.Models.Data
{
  public sealed class StationDataDay
  {
    public int Number { get; set; }
    public double CollectedTotal { get; set; }
  }
}