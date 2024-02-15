namespace CS4N.EnergyHistory.Contracts.Models.Definition
{
  public sealed class Location
  {
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public Address Address { get; set; } = new Address();
  }
}