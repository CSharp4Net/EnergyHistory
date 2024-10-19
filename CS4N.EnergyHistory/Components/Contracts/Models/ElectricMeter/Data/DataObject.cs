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

    /// <summary>
    /// Durchschnittliche Strommenge pro Tag
    /// </summary>
    public double AverageAmountPerDay
    {
      get
      {
        if (Records.Count <= 1)
          // Bei keinem oder nur einem Starteintrag erfolgt keine Berechnung
          return 0D;

        var records = Records[1..];

        return Math.Round(records.Sum(record => record.DifferenceAmountPerDay) / records.Count, 2);
      }
    }
    /// <summary>
    /// Durchschrnittlicher Stromwert pro Tag
    /// </summary>
    public decimal AverageValuePerDay
    {
      get
      {
        if (Records.Count <= 1)
          // Bei keinem oder nur einem Starteintrag erfolgt keine Berechnung
          return 0M;

        var records = Records[1..];

        return Math.Round(records.Sum(record => record.DifferenceValuePerDay) / records.Count, 2);
      }
    }
  }
}