using CS4N.EnergyHistory.MobileApp.ViewModels;

namespace CS4N.EnergyHistory.MobileApp
{
  public partial class MainPage : ContentPage
  {
    private readonly MainViewModel _vm = new();

    public MainPage()
    {
      InitializeComponent();
      BindingContext = _vm;
    }

    // OnAppearing wird jedes Mal aufgerufen, wenn die Seite sichtbar wird —
    // also auch nach dem Zurücknavigieren von der Detail- oder Einstellungsseite.
    // Das stellt sicher, dass nach einer Einstellungsänderung (Speicherort) oder
    // nach dem Erfassen neuer Daten immer frische Werte angezeigt werden.
    protected override async void OnAppearing()
    {
      base.OnAppearing();
      await _vm.LoadAsync();
      lastRefreshLabel.Text = $"Zuletzt aktualisiert: {DateTime.Now:HH:mm}";
    }

    private async void OnTileSelected(object sender, SelectionChangedEventArgs e)
    {
      if (e.CurrentSelection?.FirstOrDefault() is not TileModel tile)
        return;

      // CollectionView-Selektion sofort zurücksetzen, damit dieselbe Kachel
      // nochmals angetippt werden kann (kein visuelles "hängen bleiben").
      ((CollectionView)sender).SelectedItem = null;

      // Navigation über Shell-Routing mit GUID als Query-Parameter.
      // Das entspricht dem navigateTo() in BaseController.js.
      var route = tile.Category == "solarStation"
        ? "SolarStation.Detail"
        : "ElectricMeter.Detail";

      await Shell.Current.GoToAsync($"{route}?guid={Uri.EscapeDataString(tile.Guid)}");
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
      await Shell.Current.GoToAsync("Settings");
    }
  }
}
