namespace CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data
{
  /// <summary>
  /// Model für eine Ablesung
  /// </summary>
  public sealed class DataRecord
  {
    public int Id { get; set; }
    /// <summary>
    /// Ablesedatum (Format yyyy-MM-dd)
    /// </summary>
    public string ReadingDate { get; set; } = "";
    /// <summary>
    /// Abgelesener Wert
    /// </summary>
    public double ReadingValue { get; set; }
    /// <summary>
    /// Wertdifferenz zur vorherigen Ablesung
    /// </summary>
    public double DifferenceValue { get; set; }
    /// <summary>
    /// Tagdifferenz zur vorherigen Ablesung
    /// </summary>
    public int DifferenceDays { get; set; }

    /// <summary>
    /// Preis pro kWh
    /// </summary>
    public decimal KilowattHourPrice { get; set; }
    /// <summary>
    /// Währungseinheit
    /// </summary>
    public string CurrencyUnit { get; set; } = "€";

    /// <summary>
    /// Wert pro Tag
    /// </summary>
    public double DifferenceValuePerDay => DifferenceDays > 0D ? Math.Round(DifferenceValue / DifferenceDays, 1) : 0D;
  }
}