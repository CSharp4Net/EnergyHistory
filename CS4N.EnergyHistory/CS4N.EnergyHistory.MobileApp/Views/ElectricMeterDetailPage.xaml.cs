using CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data;
using CS4N.EnergyHistory.MobileApp.ViewModels;

namespace CS4N.EnergyHistory.MobileApp.Views
{
  // Shell übergibt den GUID-Parameter per QueryProperty-Attribut.
  // Das entspricht dem evt.getParameters().arguments.guid im UI5-Controller.
  [QueryProperty(nameof(Guid), "guid")]
  public partial class ElectricMeterDetailPage : ContentPage
  {
    private readonly ElectricMeterViewModel _vm = new();

    public string? Guid
    {
      set
      {
        if (!string.IsNullOrEmpty(value))
          _ = _vm.LoadAsync(Uri.UnescapeDataString(value));
      }
    }

    public ElectricMeterDetailPage()
    {
      InitializeComponent();
      BindingContext = _vm;
    }

    // --- Tab-Umschaltung ---

    private void OnTabRecordingClicked(object sender, EventArgs e) => ShowTab(recording: true);
    private void OnTabHistoryClicked(object sender, EventArgs e) => ShowTab(recording: false);

    private void ShowTab(bool recording)
    {
      panelRecording.IsVisible = recording;
      panelHistory.IsVisible = !recording;

      var activeColor = AppInfo.RequestedTheme == AppTheme.Dark ? Color.FromArgb("#0D1F35") : Color.FromArgb("#1C3A5E");
      var inactiveColor = AppInfo.RequestedTheme == AppTheme.Dark ? Color.FromArgb("#2C2C2E") : Color.FromArgb("#E0E0E0");
      var activeText = Colors.White;
      var inactiveText = AppInfo.RequestedTheme == AppTheme.Dark ? Color.FromArgb("#CCC") : Color.FromArgb("#444");

      tabRecording.BackgroundColor = recording ? activeColor : inactiveColor;
      tabRecording.TextColor = recording ? activeText : inactiveText;
      tabHistory.BackgroundColor = recording ? inactiveColor : activeColor;
      tabHistory.TextColor = recording ? inactiveText : activeText;
    }

    // --- Speichern ---

    private async void OnSaveClicked(object sender, EventArgs e)
    {
      // Werte aus den Eingabefeldern in das ViewModel übertragen
      _vm.NewReadingDate = datePickerInput.Date?.ToString("yyyy-MM-dd") ?? DateTime.Now.ToString("yyyy-MM-dd");
      _vm.NewReadingValue = readingValueEntry.Text ?? string.Empty;
      _vm.NewKilowattHourPrice = priceEntry.Text ?? "0";

      bool saved = await _vm.SaveNewRecordAsync();

      if (saved)
      {
        // Eingabefelder zurücksetzen
        readingValueEntry.Text = string.Empty;
        priceEntry.Text = _vm.NewKilowattHourPrice;
        datePickerInput.Date = DateTime.Today;
        await DisplayAlert("Gespeichert", "Ablesung wurde erfolgreich gespeichert.", "OK");
      }
    }

    // --- Löschen per Swipe ---

    private async void OnDeleteRecordInvoked(object sender, EventArgs e)
    {
      if (sender is SwipeItem swipe && swipe.BindingContext is ElectricMeterDataRecord record)
      {
        bool confirm = await DisplayAlert(
          "Ablesung löschen",
          $"Ablesung vom {DateTime.Parse(record.ReadingDate):dd.MM.yyyy} wirklich löschen?",
          "Löschen", "Abbrechen");

        if (confirm)
          await _vm.DeleteRecordAsync(record);
      }
    }

    private async void OnBackClicked(object sender, EventArgs e)
      => await Shell.Current.GoToAsync("..");
  }
}
