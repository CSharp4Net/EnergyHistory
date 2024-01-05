using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.Contracts.Models.Data
{
  public sealed class StationDataMonth
  {
    public StationDataMonth() { }
    public StationDataMonth(StationDefinition stationDefinition, int year, int month)
    {
      Number = month;

      PriceOfConsumedKilowattHourEnabled = stationDefinition.PriceOfConsumedKilowattHourEnabled;
      PriceOfConsumedKilowattHour = stationDefinition.PriceOfConsumedKilowattHour;

      PriceOfFedIntoKilowattHourEnabled = stationDefinition.PriceOfFedIntoKilowattHourEnabled;
      PriceOfFedIntoKilowattHour = stationDefinition.PriceOfFedIntoKilowattHour;

      //Date = new DateTime(year, month, 1);
      //Days = new List<StationDataDay>
      //{
      //  new StationDataDay(year, month, 1 ),
      //  new StationDataDay(year, month, 2 ),
      //  new StationDataDay(year, month, 3 ),
      //  new StationDataDay(year, month, 4 ),
      //  new StationDataDay(year, month, 5 ),
      //  new StationDataDay(year, month, 6 ),
      //  new StationDataDay(year, month, 7 ),
      //  new StationDataDay(year, month, 8 ),
      //  new StationDataDay(year, month, 9 ),
      //  new StationDataDay(year, month, 10),
      //  new StationDataDay(year, month, 11),
      //  new StationDataDay(year, month, 12),
      //  new StationDataDay(year, month, 13),
      //  new StationDataDay(year, month, 14),
      //  new StationDataDay(year, month, 15),
      //  new StationDataDay(year, month, 16),
      //  new StationDataDay(year, month, 17),
      //  new StationDataDay(year, month, 18),
      //  new StationDataDay(year, month, 19),
      //  new StationDataDay(year, month, 20),
      //  new StationDataDay(year, month, 21),
      //  new StationDataDay(year, month, 22),
      //  new StationDataDay(year, month, 23),
      //  new StationDataDay(year, month, 24),
      //  new StationDataDay(year, month, 25),
      //  new StationDataDay(year, month, 26),
      //  new StationDataDay(year, month, 27),
      //  new StationDataDay(year, month, 28)
      //};

      //if (month == 2 && DateTime.IsLeapYear(year))
      //  Days.Add(new StationDataDay(year, month, 29));
      //else if (month != 2)
      //{
      //  Days.Add(new StationDataDay(year, month, 29));
      //  Days.Add(new StationDataDay(year, month, 30));

      //  if (month < 8 && month % 2 == 1)
      //    Days.Add(new StationDataDay(year, month, 31));
      //  else if (month > 7 && month % 2 == 0)
      //    Days.Add(new StationDataDay(year, month, 31));
      //}
    }

    /// <summary>
    /// Monatszahl
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Gesamt erzeugte Leistung
    /// </summary>
    public double CollectedTotal { get; set; }
    /// <summary>
    /// Automatische Summierung der Tageswerte
    /// </summary>
    public bool AutomaticSummation { get; set; } = false;

    /// <summary>
    /// Preis pro verbrauchter Kilowattstunde verfügbar
    /// </summary>
    public bool PriceOfConsumedKilowattHourEnabled { get; set; } = false;
    /// <summary>
    /// Preis pro verbrauchter Kilowattstunde
    /// </summary>
    public decimal PriceOfConsumedKilowattHour { get; set; }


    /// <summary>
    /// Preis pro eingespeister Kilowattstunde verfügbar
    /// </summary>
    public bool PriceOfFedIntoKilowattHourEnabled { get; set; } = false;
    /// <summary>
    /// Preis pro eingespeister Kilowattstunde
    /// </summary>
    public decimal PriceOfFedIntoKilowattHour { get; set; }

    //public List<StationDataDay> Days { get; set; } = [];
  }
}