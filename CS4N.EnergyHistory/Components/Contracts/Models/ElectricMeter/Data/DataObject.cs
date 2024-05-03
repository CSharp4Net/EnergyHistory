namespace CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data
{
  /// <summary>
  /// Datenobjekt mit Ablesungen für einen Stromzähler
  /// </summary>
  public sealed class DataObject : DataObjectBase
  {
    /// <summary>
    /// Auflistung von Ablesungen
    /// </summary>
    public List<DataRecord> Records { get; set; } = [];

    public DataRecord? LastRecord
    {
      get
      {
        return Records
          .OrderBy(record => record.ReadingDate)
          .LastOrDefault()!;
      }
    }

    /// <summary>
    /// Datum der letzten Ablesungen
    /// </summary>
    public string LastRecordDate
    {
      get
      {
        return LastRecord?.ReadingDate ?? string.Empty;
      }
    }

    /// <summary>
    /// Datum der letzten Ablesungen
    /// </summary>
    public double LastRecordValue
    {
      get
      {
        return LastRecord?.ReadingValue ?? 0D;
      }
    }
  }
}