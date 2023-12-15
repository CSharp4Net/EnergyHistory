using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.Contracts.Models.Data
{
  public sealed class StationDataYear
  {
    public StationDataYear() { }
    public StationDataYear(int year)
    {
      Number = year;
      Months = new List<StationDataMonth>
      {
        new StationDataMonth{ Number = 1 },
        new StationDataMonth{ Number = 2 },
        new StationDataMonth{ Number = 3 },
        new StationDataMonth{ Number = 4 },
        new StationDataMonth{ Number = 5 },
        new StationDataMonth{ Number = 6 },
        new StationDataMonth{ Number = 7 },
        new StationDataMonth{ Number = 8 },
        new StationDataMonth{ Number = 9 },
        new StationDataMonth{ Number = 10 },
        new StationDataMonth{ Number = 11 },
        new StationDataMonth{ Number = 12 },
      };
    }

    public int Number { get; set; }
    public double CollectedTotal { get; set; }
    public bool ManualInput { get; set; }

    public List<StationDataMonth> Months { get; set; } = [];
  }
}