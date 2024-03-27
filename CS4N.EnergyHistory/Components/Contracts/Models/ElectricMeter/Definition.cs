namespace CS4N.EnergyHistory.Contracts.Models.ElectricMeter
{
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
    public string IconUrl { get; set; } = Constants.StationDefinitionDefaultIconUrl;

    /// <summary>
    /// Zählernummer
    /// </summary>
    public string Number { get; set; } = "";
    /// <summary>
    /// Kapazitätseinheit
    /// </summary>
    public string CapacityUnit { get; set; } = Constants.CapacityUnit_KilowattHour;

    /// <summary>
    /// Installiert am (Format yyyy-MM-dd)
    /// </summary>
    public string InstalledAt { get; set; } = "";

    /// <summary>
    /// Währungseinheit
    /// </summary>
    public string CurrencyUnit { get; set; } = "€";
    /// <summary>
    /// Anschaffungskosten
    /// </summary>
    public decimal PurchaseCosts { get; set; }
    /// <summary>
    /// Preis pro kWh
    /// </summary>
    public decimal GeneratedElectricityKilowattHourPrice { get; set; }
    /// <summary>
    /// Vergütung pro kWh
    /// </summary>
    public decimal FedInElectricityKilowattHourPrice { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<MeterUnit> Units { get; set; } = new List<MeterUnit>();
  }
}