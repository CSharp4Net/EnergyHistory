namespace CS4N.EnergyHistory.WebApp.Models.Cockpit
{
  public sealed class KpiData
  {
    public double Value { get; set; }
    public string ValueColor { get; set; } = "Neutral";
    public string Unit { get; set; } = "";
    public string IconUrl { get; set; } = "";
  }
}