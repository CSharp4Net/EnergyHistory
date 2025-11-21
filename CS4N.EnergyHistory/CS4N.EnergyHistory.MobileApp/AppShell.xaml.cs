namespace CS4N.EnergyHistory.MobileApp
{
  public partial class AppShell : Shell
  {
    public AppShell()
    {
      InitializeComponent();

      // Shell-Routen für Detailseiten registrieren
      Routing.RegisterRoute("SolarStation.Detail", typeof(Views.SolarStationDetailPage));
      Routing.RegisterRoute("ElectricMeter.Detail", typeof(Views.ElectricMeterDetailPage));
    }
  }
}
