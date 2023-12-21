namespace CS4N.EnergyHistory.Contracts.Models.Data
{
  public sealed class StationData
  {
    public string StationGuid { get; set; } = "";
    public double CollectedTotal { get; set; }
    public bool ManualInput { get; set; }
    public int ChartDataAreaId { get; set; }

    public List<StationDataYear> Years { get; set; } = [];

    public ChartDataAreaType ChartDataArea => (ChartDataAreaType)ChartDataAreaId;
  }
}