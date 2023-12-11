namespace CS4N.EnergyHistory.Contracts.Models.Definition
{
  public sealed class StationDefinition
  {
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int MaxPowerPeak { get; set; }
    public string IconUrl { get; set; } = "sap-icon://photo-voltaic";
    //public PowerUnitType PowerUnit { get; set; }
    public List<Inverter> Inverters { get; set; } = new List<Inverter>();
    public List<Module> Modules { get; set; } = new List<Module>();
    public Location? Location { get; set; }
  }
}