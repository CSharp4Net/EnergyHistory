namespace CS4N.EnergyHistory.Contracts.Models.Data
{
  public sealed class StationDataDay
  {
    public StationDataDay() { }
    public StationDataDay(int year, int month, int day)
    {
      Number = day;
      Date = new DateTime(year, month, day);
    }

    public int Number { get; set; }
    public double CollectedTotal { get; set; }

    public DateTime Date { get; init; }
  }
}