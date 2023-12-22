namespace CS4N.EnergyHistory.WebApp.ViewModels.Cockpit
{
  public sealed class GenericTileData (string category, string name, string entryViewName)
  {
    public string Category { get; set; } = category;
    public string Name { get; set; } = name;
    public string NavigationViewName { get; set; } = entryViewName;
    public string NavigationParameterAsJsonText { get; set; } = "";

    public string IconUrl { get; set; } = "";

    public KpiData? Kpi { get; set; }
    public bool KpiSet => Kpi != null;
  }
}