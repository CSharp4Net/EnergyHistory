namespace CS4N.EnergyHistory.Contracts.Models.Station.Data
{
  public sealed class StationData
  {
    /// <summary>
    /// Interne Id der Anlage
    /// </summary>
    public string StationGuid { get; set; } = "";

    /// <summary>
    /// Auflistung der Jahre
    /// </summary>
    public List<StationDataYear> Years { get; set; } = [];

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