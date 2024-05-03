using CS4N.EnergyHistory.Contracts.Models.SolarStation;

namespace CS4N.EnergyHistory.WebApp.ViewModels.SolarStation
{
  public sealed class DataView
  {
    public double GeneratedElectricityAmount { get; set; }
    public decimal GeneratedElectricityValue { get; set; }
    public decimal FedInElectricityValue { get; set; }

    public int AgeInDays => string.IsNullOrEmpty(Definition?.InstalledAt) ? 0 : (DateTime.Today - DateTime.Parse(Definition.InstalledAt)).Days;

    public double GeneratedElectricityAmountPerDay => AgeInDays == 0 ? 0D : Math.Round(GeneratedElectricityAmount / AgeInDays ,1);
    public decimal GeneratedElectricityValuePerDay => AgeInDays == 0 ? 0M : Math.Round(GeneratedElectricityValue / AgeInDays, 2);
    public decimal FedInElectricityValuePerDay => AgeInDays == 0 ? 0M : Math.Round(FedInElectricityValue / AgeInDays, 2);

    public List<ChartDataEntry> ChartData { get; set; } = [];

    public Definition? Definition { get; internal set; }
  }
}