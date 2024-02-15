namespace CS4N.EnergyHistory.Contracts.Models.Definition
{
  public sealed class Address
  {
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Street { get; set; } = "";
    public string StreetNumber { get; set; } = "";
    public string ZipCode { get; set; } = "";
    public string City { get; set; } = "";
    public string County { get; set; } = "";
    public string Country { get; set; } = "";
  }
}