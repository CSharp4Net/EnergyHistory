using CS4N.EnergyHistory.MobileApp.Services;
using System.Collections.ObjectModel;

namespace CS4N.EnergyHistory.MobileApp.ViewModels
{
  /// <summary>
  /// ViewModel für die Hauptseite (Cockpit), analog zum Cockpit.controller.js.
  /// Zeigt alle Solar-Stationen und Stromzähler als gruppierte Kacheln.
  /// </summary>
  public class MainViewModel : BaseViewModel
  {
    // ObservableCollection informiert die CollectionView automatisch über
    // Hinzufügungen und Entfernungen — kein manuelles Refresh nötig.
    public ObservableCollection<TileGroup> Groups { get; } = [];

    public MainViewModel() { }

    /// <summary>
    /// Lädt alle Stammdaten und Messwerte neu. Wird beim Erscheinen der Seite
    /// und nach dem Speichern von Einstellungen aufgerufen.
    /// </summary>
    public async Task LoadAsync()
    {
      await RunWithBusyAsync(async () =>
      {
        // Speicherdienst bei jedem Laden neu erzeugen, damit Einstellungsänderungen
        // (z.B. Umschalten local ↔ remote) sofort wirksam werden.
        var storage = StorageServiceFactory.Create();

        var solarDefinitions = await storage.GetSolarStationDefinitionsAsync();
        var electricDefinitions = await storage.GetElectricMeterDefinitionsAsync();

        // Alle UI-Änderungen müssen auf dem Main-Thread passieren
        await MainThread.InvokeOnMainThreadAsync(() => Groups.Clear());

        // --- Gruppe 1: PV-Stationen ---
        if (solarDefinitions.Count > 0)
        {
          var solarGroup = new TileGroup("PV-Stationen");

          foreach (var def in solarDefinitions)
          {
            var data = await storage.GetSolarStationDataAsync(def.Guid);

            var kpi = Math.Round(data.GeneratedElectricityAmount, 1);
            solarGroup.Add(new TileModel
            {
              Guid = def.Guid,
              Category = "solarStation",
              Name = def.Name,
              KpiText = $"{kpi:N1} {def.CapacityUnit}",
              KpiColor = Colors.ForestGreen,
              FooterText = data.GeneratedElectricityValue > 0
                ? $"Ertrag: {data.GeneratedElectricityValue:C2}"
                : string.Empty
            });
          }

          await MainThread.InvokeOnMainThreadAsync(() => Groups.Add(solarGroup));
        }

        // --- Gruppe 2: Stromzähler ---
        if (electricDefinitions.Count > 0)
        {
          var meterGroup = new TileGroup("Stromzähler");

          foreach (var def in electricDefinitions)
          {
            var data = await storage.GetElectricMeterDataAsync(def.Guid);

            var displayName = string.IsNullOrEmpty(def.Name) ? def.Number : def.Name;
            var lastValue = data.LastRecordValue;

            meterGroup.Add(new TileModel
            {
              Guid = def.Guid,
              Category = "electricMeter",
              Name = displayName,
              KpiText = $"{lastValue:N1} {def.CapacityUnit}",
              KpiColor = def.IsConsumptionMeter ? Colors.OrangeRed : Colors.SteelBlue,
              FooterText = data.LastRecordDate.Length > 0
                ? $"Stand: {DateTime.Parse(data.LastRecordDate):dd.MM.yyyy}"
                : "Noch keine Ablesung"
            });
          }

          await MainThread.InvokeOnMainThreadAsync(() => Groups.Add(meterGroup));
        }
      });
    }
  }

  // --- Datenmodelle für die Cockpit-Kacheln ---

  /// <summary>
  /// Eine Gruppe von Kacheln (z.B. "PV-Stationen").
  /// Ableitung von ObservableCollection ermöglicht die Verwendung als
  /// IsGrouped-Quelle in CollectionView, analog zu SAP UI5 GroupHeaderTemplate.
  /// </summary>
  public class TileGroup(string key) : ObservableCollection<TileModel>
  {
    public string Key { get; } = key;
  }

  public class TileModel
  {
    public required string Guid { get; init; }
    public required string Category { get; init; }
    public required string Name { get; init; }
    public required string KpiText { get; init; }
    public required Color KpiColor { get; init; }
    public string FooterText { get; init; } = string.Empty;
  }
}
