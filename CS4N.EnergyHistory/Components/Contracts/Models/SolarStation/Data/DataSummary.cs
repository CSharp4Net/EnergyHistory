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
    public double GeneratedElectricityAmount => Years.Sum(year => year.GeneratedElectricityAmount);
    /// <summary>
    /// Erzeugter Stromwert über alle Jahre
    /// </summary>
    public decimal GeneratedElectricityValue => Years.Sum(year => year.GeneratedElectricityValue);
    /// <summary>
    /// Eingespeister Stromwert über alle Jahre
    /// </summary>
    public decimal FedInElectricityValue => Years.Sum(year => year.FedInElectricityValue);
  }
}