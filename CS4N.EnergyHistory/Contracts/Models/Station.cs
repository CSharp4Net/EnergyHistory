namespace CS4N.EnergyHistory.Contracts.Models
{
  public sealed class Station
  {
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public double MaxPowerPeak { get; set; }
    //public PowerUnitType PowerUnit { get; set; }
    public List<Inverter> Inverters { get; set; } = new List<Inverter>();
    public List<Module> Modules { get; set; } = new List<Module>();
    public Location? Location { get; set; }
  }
}