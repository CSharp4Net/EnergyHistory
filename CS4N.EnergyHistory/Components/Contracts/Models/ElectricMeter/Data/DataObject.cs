namespace CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data
{
  public sealed class DataObject : DataObjectBase
  {
    public List<DataRecord> Records { get; set; } = [];

    //public double LastRecordValue => Records.OrderBy(record => record.ReadingDate).LastOrDefault()?.Value ?? 0D;
  }
}