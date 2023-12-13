using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.Contracts.Models.Data
{
  public sealed class StationDataMonth
  {
    public int Number { get; set; }
    public double CollectedTotal { get; set; }
    public bool ManualInput { get; set; }

    public List<StationDataDay> Days { get; set; } = [];
  }
}