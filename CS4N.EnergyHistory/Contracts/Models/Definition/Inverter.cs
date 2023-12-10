namespace CS4N.EnergyHistory.Contracts.Models.Definition
{
  public sealed class Inverter
  {
    public int Id { get; set; }
    public string Manufacturer { get; set; } = "";
    public string Model { get; set; } = "";
    public string SerialNumber { get; set; } = "";
    public int MaxPowerPeak { get; set; }
    //public PowerUnitType PowerUnit { get; set; }
  }
}