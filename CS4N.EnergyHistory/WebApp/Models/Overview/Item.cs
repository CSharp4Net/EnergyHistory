namespace CS4N.EnergyHistory.WebApp.Models.Overview
{
  public sealed class Item (string category, string name, string entryViewName)
  {
    public string Category { get; set; } = category;
    public string Name { get; set; } = name;
    public string EntryViewName { get; set; } = entryViewName;
  }
}