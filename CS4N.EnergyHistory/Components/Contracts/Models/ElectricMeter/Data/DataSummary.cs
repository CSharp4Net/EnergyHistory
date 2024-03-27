namespace CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data
{
  public sealed class DataSummary : DataObjectBase
  {
    public List<DataRecord> Records { get; set; } = [];

    /// <summary>
    /// Eingespeister Stromwert über alle Jahre
    /// </summary>
    public double LastRecordValue => Records.OrderBy(record => record.ReadingDate).LastOrDefault()?.Value ?? 0D;
  }
}