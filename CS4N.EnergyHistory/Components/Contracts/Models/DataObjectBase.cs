namespace CS4N.EnergyHistory.Contracts.Models
{
  public abstract class DataObjectBase
  {
    /// <summary>
    /// Interne Id der Anlage
    /// </summary>
    public string StationGuid { get; set; } = "";
  }
}