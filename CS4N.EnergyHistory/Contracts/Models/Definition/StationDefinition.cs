namespace CS4N.EnergyHistory.Contracts.Models.Definition
{
  public sealed class StationDefinition
  {
    public string Guid { get; set; } = "";
    public string Name { get; set; } = "";
    public double MaxWattPeak { get; set; }
    public string IconUrl { get; set; } = Constants.StationDefinitionDefaultIconUrl;
    public string PowerUnit { get; set; } = Constants.PowerUnit_Watt;
    public string CapacityUnit { get; set; } = Constants.CapacityUnit_KilowattHour;
    public string InstalledAt { get; set; } = "";
    public string CurrencyUnit { get; set; } = "€";
    public decimal PurchaseCosts { get; set; }
    public decimal PriceOfConsumedKilowattHour { get; set; }
    public decimal PriceOfFedIntoKilowattHour { get; set; }

    public Location? Location { get; set; }
    public List<Inverter> Inverters { get; set; } = new List<Inverter>();
    public List<Module> Modules { get; set; } = new List<Module>();

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }
}