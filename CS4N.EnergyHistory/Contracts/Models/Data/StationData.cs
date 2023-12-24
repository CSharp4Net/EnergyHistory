using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.Contracts.Models.Data
{
  public sealed class StationData
  {
    /// <summary>
    /// Interne Id der Anlage
    /// </summary>
    public string StationGuid { get; set; } = "";
    /// <summary>
    /// Gesamt erzeugte Leistung
    /// </summary>
    public double CollectedTotal { get; set; }
    /// <summary>
    /// Automatische Summierung der Monatswerte
    /// </summary>
    public bool AutomaticSummation { get; set; } = true;
    /// <summary>
    /// Standardansicht des Auswertungsdiagramm
    /// </summary>
    public string DefaultChartAreaType { get; set; } = Constants.StationDataDefaultChartAreaType;

    /// <summary>
    /// Auflistung der Jahre
    /// </summary>
    public List<StationDataYear> Years { get; set; } = [];
  }
}