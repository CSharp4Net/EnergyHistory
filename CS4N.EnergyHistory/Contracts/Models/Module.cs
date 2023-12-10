namespace CS4N.EnergyHistory.Contracts.Models
{
  public sealed class Module
  {
    public int Id { get; set; }
    public string Manufacturer { get; set; } = "";
    public string Model { get; set; } = "";
    public string SerialNumber { get; set; } = "";
    public double MaxPowerPeak { get; set; }
    //public PowerUnitType PowerUnit { get; set; }
  }
}