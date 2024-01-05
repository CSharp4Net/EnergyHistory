namespace CS4N.EnergyHistory.WebApp.ViewModels.Station
{
  public sealed class ChartDataFilter
  {
    public string StepType { get; set; } = ChartDataStepType.Month.ToString();
    public ChartDataStepType StepTypeEnum => Enum.Parse<ChartDataStepType>(StepType);
    
    public string DateFrom { get; set; } = DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd");
    public string DateTo { get; set; } = DateTime.Today.ToString("yyyy-MM-dd");
  }
}