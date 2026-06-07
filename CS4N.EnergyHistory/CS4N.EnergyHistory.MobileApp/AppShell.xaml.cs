using CS4N.EnergyHistory.MobileApp.Views;

namespace CS4N.EnergyHistory.MobileApp
{
  public partial class AppShell : Shell
  {
    public AppShell()
    {
      InitializeComponent();

      // Detail-Routen registrieren — diese Seiten werden per GoToAsync angesteuert.
      // Das Schema entspricht den SAP UI5 Router-Routen aus manifest.json, nur
      // dass MAUI URL-Parameter (QueryProperty) statt Pfad-Pattern nutzt.
      Routing.RegisterRoute("SolarStation.Detail", typeof(SolarStationDetailPage));
      Routing.RegisterRoute("ElectricMeter.Detail", typeof(ElectricMeterDetailPage));
      Routing.RegisterRoute("Settings", typeof(SettingsPage));
    }
  }
}
