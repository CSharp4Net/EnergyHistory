namespace CS4N.EnergyHistory.Contracts.Models.SolarStation.Data
{
  public sealed class DataSummary : DataObjectBase
  {
    /// <summary>
    /// Auflistung der Jahre
    /// </summary>
    public List<DataOfYear> Years { get; set; } = [];

    /// <summary>
    /// Insgesamt erzeugter Strom
    /// </summary>
    public double GeneratedElectricityAmount => Math.Round(Years.Sum(year => year.GeneratedElectricityAmount), 2);
    /// <summary>
    /// Erzeugter Stromwert über alle Jahre
    /// </summary>
    public decimal GeneratedElectricityValue => Math.Round(Years.Sum(year => year.GeneratedElectricityValue), 2);
    /// <summary>
    /// Eingespeister Stromwert über alle Jahre
    /// </summary>
    public decimal FedInElectricityValue => Math.Round(Years.Sum(year => year.FedInElectricityValue), 2);
  }
}