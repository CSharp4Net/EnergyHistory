using CS4N.EnergyHistory.Contracts.Models.Definition;
using System.ComponentModel.Design;

namespace CS4N.EnergyHistory.Contracts.Models.Data
{
  public sealed class StationDataMonth
  {
    public StationDataMonth() { }
    public StationDataMonth(int year, int month)
    {
      Number = month;
      Days = new List<StationDataDay>
      {
        new StationDataDay{ Number = 1 },
        new StationDataDay{ Number = 2 },
        new StationDataDay{ Number = 3 },
        new StationDataDay{ Number = 4 },
        new StationDataDay{ Number = 5 },
        new StationDataDay{ Number = 6 },
        new StationDataDay{ Number = 7 },
        new StationDataDay{ Number = 8 },
        new StationDataDay{ Number = 9 },
        new StationDataDay{ Number = 10 },
        new StationDataDay{ Number = 11 },
        new StationDataDay{ Number = 12 },
        new StationDataDay{ Number = 13 },
        new StationDataDay{ Number = 14 },
        new StationDataDay{ Number = 15 },
        new StationDataDay{ Number = 16 },
        new StationDataDay{ Number = 17 },
        new StationDataDay{ Number = 18 },
        new StationDataDay{ Number = 19 },
        new StationDataDay{ Number = 20 },
        new StationDataDay{ Number = 21 },
        new StationDataDay{ Number = 22 },
        new StationDataDay{ Number = 23 },
        new StationDataDay{ Number = 24 },
        new StationDataDay{ Number = 25 },
        new StationDataDay{ Number = 26 },
        new StationDataDay{ Number = 27 },
        new StationDataDay{ Number = 28 },
      };

      if (month == 2 && DateTime.IsLeapYear(year))
        Days.Add(new StationDataDay { Number = 29 });
      else if (month != 2)
      {
        Days.Add(new StationDataDay { Number = 29 });
        Days.Add(new StationDataDay { Number = 30 });

        if (month < 8 && month % 2 == 1)
          Days.Add(new StationDataDay { Number = 31 });
        else if (month > 7 && month % 2 == 0)
          Days.Add(new StationDataDay { Number = 31 });
      }
    }

    public int Number { get; set; }
    public double CollectedTotal { get; set; }
    public bool ManualInput { get; set; }

    public List<StationDataDay> Days { get; set; } = [];
  }
}