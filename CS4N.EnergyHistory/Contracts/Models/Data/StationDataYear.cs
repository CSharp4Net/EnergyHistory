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
        new StationDataMonth(year, 1 ),
        new StationDataMonth(year, 2 ),
        new StationDataMonth(year, 3 ),
        new StationDataMonth(year, 4 ),
        new StationDataMonth(year, 5 ),
        new StationDataMonth(year, 6 ),
        new StationDataMonth(year, 7 ),
        new StationDataMonth(year, 8 ),
        new StationDataMonth(year, 9 ),
        new StationDataMonth(year, 10),
        new StationDataMonth(year, 11),
        new StationDataMonth(year, 12)
      };
    }

    public int Number { get; set; }
    public double CollectedTotal { get; set; }
    public bool AutomaticSummation { get; set; } = true;

    public List<StationDataMonth> Months { get; set; } = [];
  }
}