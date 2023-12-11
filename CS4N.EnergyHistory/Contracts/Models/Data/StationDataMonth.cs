namespace CS4N.EnergyHistory.Contracts.Models.Data
{
  public sealed class StationDataMonth
  {
    public int Id { get; set; }
    public int StationId { get; set; }
    public int Year { get; set; }
    public int Number { get; set; }
    public int PowerTotal { get; set; }

    public List<StationDataDay> Days { get; set; } = [];
  }
}