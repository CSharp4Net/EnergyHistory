namespace CS4N.EnergyHistory.WebApp.Models.Cockpit
{
  public sealed class GenericTileData (string category, string name, string entryViewName)
  {
    public string Category { get; set; } = category;
    public string Name { get; set; } = name;
    public string NavigationViewName { get; set; } = entryViewName;
    public string NavigationParameterAsJsonText { get; set; } = "";

    public string IconUrl { get; set; } = "";

    public string KpiUnit { get; set; } = "";
    public string KpiValue { get; set; } = "";
  }
}