namespace CS4N.EnergyHistory.WebApp.Models.Station
{
  public sealed class ViewData
  {
    public string StationId { get; set; } = "";
    public string StationName { get; set; } = "";
    public double StationMaxWattPeak { get; set; }
    public double StationCollectedTotal { get; set; }

    public List<ChartDataEntry> ChartData { get; set; } = [];
  }
}