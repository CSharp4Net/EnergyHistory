using CS4N.EnergyHistory.MobileApp.ViewModels;

namespace CS4N.EnergyHistory.MobileApp.Views
{
  [QueryProperty(nameof(Guid), "guid")]
  public partial class SolarStationDetailPage : ContentPage
  {
    private readonly SolarStationViewModel _vm = new();

    public string? Guid
    {
      set
      {
        if (!string.IsNullOrEmpty(value))
          _ = _vm.LoadAsync(Uri.UnescapeDataString(value));
      }
    }

    public SolarStationDetailPage()
    {
      InitializeComponent();
      BindingContext = _vm;
    }

    // --- Tab-Umschaltung ---

    private void OnTabSummaryClicked(object sender, EventArgs e) => ShowTab(summary: true);
    private void OnTabEntryClicked(object sender, EventArgs e) => ShowTab(summary: false);

    private void ShowTab(bool summary)
    {
      panelSummary.IsVisible = summary;
      panelEntry.IsVisible = !summary;

      var activeColor = Color.FromArgb("#1B5E20");
      var inactiveColor = AppInfo.RequestedTheme == AppTheme.Dark ? Color.FromArgb("#2C2C2E") : Color.FromArgb("#E0E0E0");

      tabSummary.BackgroundColor = summary ? activeColor : inactiveColor;
      tabSummary.TextColor = summary ? Colors.White : (AppInfo.RequestedTheme == AppTheme.Dark ? Color.FromArgb("#CCC") : Color.FromArgb("#444"));
      tabEntry.BackgroundColor = summary ? inactiveColor : activeColor;
      tabEntry.TextColor = summary ? (AppInfo.RequestedTheme == AppTheme.Dark ? Color.FromArgb("#CCC") : Color.FromArgb("#444")) : Colors.White;
    }

    // --- Neues Jahr hinzufügen ---

    private async void OnAddYearClicked(object sender, EventArgs e)
    {
      // Benutzer nach der Jahreszahl fragen
      string? input = await DisplayPromptAsync(
        "Neues Jahr",
        "Für welches Jahr möchtest du Daten erfassen?",
        initialValue: DateTime.Today.Year.ToString(),
        keyboard: Keyboard.Numeric);

      if (!int.TryParse(input, out int year)) return;

      // Prüfen ob das Jahr bereits existiert
      if (_vm.Years.Any(y => y.Year == year))
      {
        await DisplayAlert("Hinweis", $"Jahr {year} ist bereits vorhanden.", "OK");
        return;
      }

      // Neues Jahr-Modell wird im ViewModel erstellt und der Liste hinzugefügt.
      // Da Years eine ObservableCollection ist, aktualisiert sich die CollectionView automatisch.
      if (_vm.Definition != null)
      {
        var newRow = new YearRowModel(year, _vm.Definition);
        _vm.Years.Insert(0, newRow);
      }
    }

    // --- Jahreswerte speichern ---

    private async void OnSaveYearClicked(object sender, EventArgs e)
    {
      if (sender is Button btn && btn.CommandParameter is YearRowModel yearRow)
      {
        await _vm.SaveYearAsync(yearRow);
        await DisplayAlert("Gespeichert", $"Daten für {yearRow.Year} wurden gespeichert.", "OK");
      }
    }

    private async void OnBackClicked(object sender, EventArgs e)
      => await Shell.Current.GoToAsync("..");
  }
}
