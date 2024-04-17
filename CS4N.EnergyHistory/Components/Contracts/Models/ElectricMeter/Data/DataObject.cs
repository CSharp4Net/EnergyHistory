namespace CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data
{
  public sealed class DataObject : DataObjectBase
  {
    public List<DataRecord> Records { get; set; } = [];

    public DataRecord? LastRecord
    {
      get
      {
        return Records.OrderBy(record => record.ReadingDate).LastOrDefault();
      }
    }

    public string LastRecordDate
    {
      get
      {
        return LastRecord?.ReadingDate ?? string.Empty;
      }
    }
    public double LastRecordValue
    {
      get
      {
        return LastRecord?.Value ?? 0D;
      }
    }
  }
}