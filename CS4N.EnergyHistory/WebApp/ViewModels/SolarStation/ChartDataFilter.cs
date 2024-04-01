namespace CS4N.EnergyHistory.WebApp.ViewModels.SolarStation
{
  public sealed class ChartDataFilter
  {
    public string StepType { get; set; } = SolarStationChartDataStepType.Month.ToString();
    public SolarStationChartDataStepType StepTypeEnum => Enum.Parse<SolarStationChartDataStepType>(StepType);
    
    public string DateFrom { get; set; } = DateTime.Today.AddMonths(-11).ToString("yyyy-MM-dd");
    public string DateTo { get; set; } = DateTime.Today.ToString("yyyy-MM-dd");
  }
}