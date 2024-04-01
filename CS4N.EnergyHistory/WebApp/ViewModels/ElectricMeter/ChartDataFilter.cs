namespace CS4N.EnergyHistory.WebApp.ViewModels.ElectricMeter
{
  public sealed class ChartDataFilter
  {
    public string StepType { get; set; } = ElectricMeterChartDataStepType.Year.ToString();
    public ElectricMeterChartDataStepType StepTypeEnum => Enum.Parse<ElectricMeterChartDataStepType>(StepType);
    
    public string DateFrom { get; set; } = DateTime.Today.AddYears(-10).ToString("yyyy-MM-dd");
    public string DateTo { get; set; } = DateTime.Today.ToString("yyyy-MM-dd");
  }
}