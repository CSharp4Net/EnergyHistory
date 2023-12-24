using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.Contracts.Models.Data
{
  public sealed class StationDataYear
  {
    public StationDataYear() { }
    public StationDataYear(StationDefinition stationDefinition, int year)
    {
      Number = year;

      PriceOfConsumedKilowattHour = stationDefinition.PriceOfConsumedKilowattHour;
      PriceOfFedIntoKilowattHour = stationDefinition.PriceOfFedIntoKilowattHour;

      Months = new List<StationDataMonth>
      {
        new StationDataMonth(stationDefinition, year, 1 ),
        new StationDataMonth(stationDefinition, year, 2 ),
        new StationDataMonth(stationDefinition, year, 3 ),
        new StationDataMonth(stationDefinition, year, 4 ),
        new StationDataMonth(stationDefinition, year, 5 ),
        new StationDataMonth(stationDefinition, year, 6 ),
        new StationDataMonth(stationDefinition, year, 7 ),
        new StationDataMonth(stationDefinition, year, 8 ),
        new StationDataMonth(stationDefinition, year, 9 ),
        new StationDataMonth(stationDefinition, year, 10),
        new StationDataMonth(stationDefinition, year, 11),
        new StationDataMonth(stationDefinition, year, 12)
      };
    }

    /// <summary>
    /// Jahreszahl
    /// </summary>
    public int Number { get; set; }
    /// <summary>
    /// Gesamt erzeugte Leistung
    /// </summary>
    public double CollectedTotal { get; set; }
    /// <summary>
    /// Automatische Summierung der Monatswerte
    /// </summary>
    public bool AutomaticSummation { get; set; } = true;

    /// <summary>
    /// Preis pro verbrauchter Kilowattstunde
    /// </summary>
    public decimal PriceOfConsumedKilowattHour { get; set; }
    /// <summary>
    /// Produkt aus <see cref="CollectedTotal"/> und <see cref="PriceOfConsumedKilowattHour"/>
    /// </summary>
    public decimal CollectedTotalByConsumedPrice { get; set; }

    /// <summary>
    /// Preis pro eingespeißter Kilowattstunde
    /// </summary>
    public decimal PriceOfFedIntoKilowattHour { get; set; }
    /// <summary>
    /// Produkt aus <see cref="CollectedTotal"/> und <see cref="PriceOfFedIntoKilowattHour"/>
    /// </summary>
    public decimal CollectedTotalFedIntoPrice { get; set; }

    /// <summary>
    /// Auflistung der Monate
    /// </summary>
    public List<StationDataMonth> Months { get; set; } = [];
  }
}