namespace CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data
{
  public sealed class DataRecord
  {
    public DateTime ReadingDate { get; set; }
    public double Value { get; set; }
  }
}