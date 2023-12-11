using System.Text.Json.Serialization;

namespace CS4N.EnergyHistory.Contracts.Models.Data
{
  public sealed class StationDataYear
  {
    public int Number { get; set; }
    public int PowerTotal { get; set; }

    public List<StationDataMonth> Months { get; set; } = [];
  }
}