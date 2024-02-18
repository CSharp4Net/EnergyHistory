namespace CS4N.EnergyHistory.Contracts.Models
{
  public sealed class NameValuePair (string name, string value)
  {
    public string Name { get; set; } = name;
    public string Value { get; set; } = value;
  }
}