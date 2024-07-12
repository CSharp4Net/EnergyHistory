namespace CS4N.EnergyHistory.Contracts.Models.SolarStation
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
    public string IconUrl { get; set; } = Constants.SolarStationDefinitionDefaultIconUrl;

    /// <summary>
    /// Leistungsspitze
    /// </summary>
    public double PowerPeak { get; set; }
    /// <summary>
    /// Leistungseinheit
    /// </summary>
    public string PowerUnit { get; set; } = Constants.PowerUnit_Watt;
    /// <summary>
    /// Kapazitätseinheit
    /// </summary>
    public string CapacityUnit { get; set; } = Constants.CapacityUnit_KilowattHour;

    /// <summary>
    /// Installiert am (Format yyyy-MM-dd)
    /// </summary>
    public string InstalledAt { get; set; } = "";
    /// <summary>
    /// Allgemeine Bemerkungen
    /// </summary>
    public string CommonComments { get; set; } = "";

    /// <summary>
    /// Währungseinheit
    /// </summary>
    public string CurrencyUnit { get; set; } = "€";
    /// <summary>
    /// Anschaffungskosten
    /// </summary>
    public decimal PurchaseCosts { get; set; }

    /// <summary>
    /// Anzeige und Berechnung Verbrauch pro kWh
    /// </summary>
    public bool GeneratedElectricityEnabled { get; set; } = true;
    /// <summary>
    /// Preis pro kWh
    /// </summary>
    public decimal GeneratedElectricityKilowattHourPrice { get; set; }
    /// <summary>
    /// Anzeige und Bereich Vergütung pro kWh
    /// </summary>
    public bool FedInEnabled { get; set; } = true;
    /// <summary>
    /// Vergütung pro kWh
    /// </summary>
    public decimal FedInElectricityKilowattHourPrice { get; set; }
    /// <summary>
    /// Finanzielle Bemerkungen
    /// </summary>
    public string FinanceComments { get; set; } = "";
    /// <summary>
    /// API-Verbindungsdefinition
    /// </summary>
    public ApiDefinition ApiDefinition { get; set; } = new ApiDefinition();

    //public Location? Location { get; set; }
    //public List<Inverter> Inverters { get; set; } = new List<Inverter>();
    //public List<Module> Modules { get; set; } = new List<Module>();

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }
}