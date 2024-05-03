namespace CS4N.EnergyHistory.Contracts.Models.ElectricMeter
{
  /// <summary>
  /// Model für eine Stromzähleinheit
  /// </summary>
  public sealed class Definition
  {
    /// <summary>
    /// Eindeutige ID
    /// </summary>
    public string Guid { get; set; } = "";
    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = "";
    /// <summary>
    /// ICON-Url
    /// </summary>
    public string IconUrl { get; set; } = Constants.ElectricMeterDefinitionDefaultIconUrl;

    /// <summary>
    /// Installiert am (Format yyyy-MM-dd)
    /// </summary>
    public string InstalledAt { get; set; } = "";

    /// <summary>
    /// Zählernummer
    /// </summary>
    public string Number { get; set; } = "";
    /// <summary>
    /// Zähleinheit
    /// </summary>
    public string UnitCode { get; set; } = "2.8.0";
    /// <summary>
    /// Kapazitätseinheit
    /// </summary>
    public string CapacityUnit { get; set; } = Constants.CapacityUnit_KilowattHour;

    /// <summary>
    /// Wenn True, handelt es sich um einen Verbrauchszähler, andernfalls um einen Einspeisezähler.
    /// </summary>
    public bool IsConsumptionMeter { get; set; }

    /// <summary>
    /// Preis pro kWh
    /// </summary>
    public decimal KilowattHourPrice { get; set; }
    /// <summary>
    /// Währungseinheit
    /// </summary>
    public string CurrencyUnit { get; set; } = "€";

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }
}