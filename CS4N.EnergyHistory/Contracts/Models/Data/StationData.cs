using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.Contracts.Models.Data
{
  public sealed class StationData
  {
    public string StationGuid { get; set; } = "";
    public double CollectedTotal { get; set; }
    public bool AutomaticSummation { get; set; } = true;
    public string ChartAreaType { get; set; } = Constants.StationDataDefaultChartAreaType;

    public List<StationDataYear> Years { get; set; } = [];
  }
}