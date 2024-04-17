namespace CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data
{
  public sealed class DataRecord 
  {
    public string MeterUnitCode { get; set; } = "";
    public string ReadingDate { get; set; } = "";
    public double Value { get; set; }
  }
}