namespace CS4N.EnergyHistory.Contracts.Models.SolarStation.Data
{
  public sealed class DataOfYear
  {
    public DataOfYear() { }
    public DataOfYear(Definition stationDefinition, int year)
    {
      Number = year;

      GeneratedElectricityEnabled = stationDefinition.GeneratedElectricityEnabled;
      GeneratedElectricityKilowattHourPrice = stationDefinition.GeneratedElectricityKilowattHourPrice;

      FedInEnabled = stationDefinition.FedInEnabled;
      FedInElectricityKilowattHourPrice = stationDefinition.FedInElectricityKilowattHourPrice;

      Months = new List<DataOfMonth>
      {
        new DataOfMonth(stationDefinition, year, 1 ),
        new DataOfMonth(stationDefinition, year, 2 ),
        new DataOfMonth(stationDefinition, year, 3 ),
        new DataOfMonth(stationDefinition, year, 4 ),
        new DataOfMonth(stationDefinition, year, 5 ),
        new DataOfMonth(stationDefinition, year, 6 ),
        new DataOfMonth(stationDefinition, year, 7 ),
        new DataOfMonth(stationDefinition, year, 8 ),
        new DataOfMonth(stationDefinition, year, 9 ),
        new DataOfMonth(stationDefinition, year, 10),
        new DataOfMonth(stationDefinition, year, 11),
        new DataOfMonth(stationDefinition, year, 12)
      };
    }

    /// <summary>
    /// Jahreszahl
    /// </summary>
    public int Number { get; set; }
    /// <summary>
    /// Automatische Summierung der Monatswerte
    /// </summary>
    public bool AutomaticSummation { get; set; } = true;

    /// <summary>
    /// Gesamt erzeugte Leistung
    /// </summary>
    public double GeneratedElectricityAmount { get; set; }

    /// <summary>
    /// Preis pro verbrauchter Kilowattstunde verfügbar
    /// </summary>
    public bool GeneratedElectricityEnabled { get; set; } = false;
    /// <summary>
    /// Preis pro verbrauchter Kilowattstunde
    /// </summary>
    public decimal GeneratedElectricityKilowattHourPrice { get; set; }
    /// <summary>
    /// Produkt aus <see cref="GeneratedElectricityAmount"/> * <see cref="GeneratedElectricityKilowattHourPrice"/>
    /// </summary>
    public decimal GeneratedElectricityValue => Convert.ToDecimal(GeneratedElectricityAmount) * GeneratedElectricityKilowattHourPrice;

    /// <summary>
    /// Preis pro eingespeister Kilowattstunde verfügbar
    /// </summary>
    public bool FedInEnabled { get; set; } = false;
    /// <summary>
    /// Preis pro eingespeister Kilowattstunde
    /// </summary>
    public decimal FedInElectricityKilowattHourPrice { get; set; }
    /// <summary>
    /// Produkt aus <see cref="GeneratedElectricityAmount"/> * <see cref="FedInElectricityKilowattHourPrice"/>
    /// </summary>
    public decimal FedInElectricityValue => Convert.ToDecimal(GeneratedElectricityAmount) * FedInElectricityKilowattHourPrice;

    /// <summary>
    /// Auflistung der Monate
    /// </summary>
    public List<DataOfMonth> Months { get; set; } = [];

    public string Comments { get; set; } = "";
  }
}