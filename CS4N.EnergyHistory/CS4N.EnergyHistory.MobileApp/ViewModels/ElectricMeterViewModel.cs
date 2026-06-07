using CS4N.EnergyHistory.Contracts.Models.ElectricMeter;
using CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data;
using CS4N.EnergyHistory.MobileApp.Services;
using System.Collections.ObjectModel;

namespace CS4N.EnergyHistory.MobileApp.ViewModels
{
  /// <summary>
  /// ViewModel für die Stromzähler-Detailseite.
  /// Entspricht dem ElectricMeterData.controller.js aus der WebApp, nun jedoch
  /// typsicher in C# mit MVVM-Datenbindung statt JavaScript-Modelpfaden.
  /// </summary>
  public class ElectricMeterViewModel : BaseViewModel
  {
    private IStorageService? _storage;

    // --- Stammdaten der Definition ---

    private ElectricMeterDefinition? _definition;
    public ElectricMeterDefinition? Definition
    {
      get => _definition;
      set
      {
        SetProperty(ref _definition, value);
        // Wenn sich die Definition ändert, wird der Neueingabe-Datensatz
        // mit dem aktuellen kWh-Preis vorbelegt — analog zu buildNewRecord() im Controller
        if (value != null)
          BuildNewRecord();
      }
    }

    // --- Ablesedaten ---

    private ElectricMeterDataObject? _data;
    public ElectricMeterDataObject? Data
    {
      get => _data;
      set
      {
        SetProperty(ref _data, value);
        // Listenansicht bei jeder Datenänderung neu befüllen
        RefreshRecordList();
      }
    }

    // Für die Historien-Liste in CollectionView — sortiert absteigend nach Datum
    public ObservableCollection<ElectricMeterDataRecord> Records { get; } = [];

    // --- Zusammenfassende KPI-Werte für den Kopfbereich ---

    public string LastRecordDateDisplay =>
      _data?.LastRecordDate is { Length: > 0 } d
        ? DateTime.Parse(d).ToString("dddd, dd. MMMM yyyy")
        : "Noch keine Ablesung";

    public string LastRecordValueDisplay =>
      $"{_data?.LastRecordValue:N1} {_definition?.CapacityUnit}";

    public string AverageAmountDisplay =>
      $"{_data?.AverageAmountPerDay:N2} {_definition?.CapacityUnit}/Tag";

    public string AverageValueDisplay =>
      $"{_data?.AverageValuePerDay:N2} {_definition?.CurrencyUnit}/Tag";

    // --- Neuer Eintrag (Eingabeformular) ---

    private string _newReadingDate = DateTime.Today.ToString("yyyy-MM-dd");
    public string NewReadingDate
    {
      get => _newReadingDate;
      set => SetProperty(ref _newReadingDate, value);
    }

    private string _newReadingValue = string.Empty;
    public string NewReadingValue
    {
      get => _newReadingValue;
      set => SetProperty(ref _newReadingValue, value);
    }

    private string _newKilowattHourPrice = "0";
    public string NewKilowattHourPrice
    {
      get => _newKilowattHourPrice;
      set => SetProperty(ref _newKilowattHourPrice, value);
    }

    // --- Validierungs-Fehlermeldungen ---
    private string _dateError = string.Empty;
    public string DateError { get => _dateError; set => SetProperty(ref _dateError, value); }

    private string _valueError = string.Empty;
    public string ValueError { get => _valueError; set => SetProperty(ref _valueError, value); }

    // --- Laden ---

    public async Task LoadAsync(string guid)
    {
      await RunWithBusyAsync(async () =>
      {
        _storage = StorageServiceFactory.Create();

        var definitions = await _storage.GetElectricMeterDefinitionsAsync();
        Definition = definitions.FirstOrDefault(d => d.Guid == guid);

        Data = await _storage.GetElectricMeterDataAsync(guid);
      });
    }

    // --- Neue Ablesung speichern ---

    public async Task<bool> SaveNewRecordAsync()
    {
      // Validierung, analog zu onAddPress() im Controller
      bool valid = true;
      DateError = string.Empty;
      ValueError = string.Empty;

      if (!DateTime.TryParse(NewReadingDate, out var parsedDate))
      {
        DateError = "Bitte ein gültiges Datum eingeben.";
        valid = false;
      }

      if (!double.TryParse(NewReadingValue, out var parsedValue) || parsedValue < 0)
      {
        ValueError = "Bitte einen gültigen positiven Zählerstand eingeben.";
        valid = false;
      }

      if (!valid) return false;

      _ = decimal.TryParse(NewKilowattHourPrice, out var parsedPrice);

      await RunWithBusyAsync(async () =>
      {
        // Neuen Datensatz anhängen und Differenzwerte berechnen
        var records = Data!.Records;
        var lastRecord = records.OrderBy(r => r.ReadingDate).LastOrDefault();

        var newRecord = new ElectricMeterDataRecord
        {
          Id = records.Count == 0 ? 1 : records.Max(r => r.Id) + 1,
          ReadingDate = parsedDate.ToString("yyyy-MM-dd"),
          ReadingValue = parsedValue,
          KilowattHourPrice = parsedPrice,
          CurrencyUnit = _definition?.CurrencyUnit ?? "€"
        };

        // Differenz zur vorherigen Ablesung berechnen
        if (lastRecord != null && DateTime.TryParse(lastRecord.ReadingDate, out var lastDate))
        {
          newRecord.DifferenceValue = Math.Round(parsedValue - lastRecord.ReadingValue, 2);
          newRecord.DifferenceDays = (parsedDate - lastDate).Days;
        }

        Data!.Records.Add(newRecord);

        await _storage!.UpsertElectricMeterDataAsync(Data);

        // ViewModel aktualisieren
        Data = Data; // Trigger PropertyChanged für abgeleitete Werte
        BuildNewRecord();
      });

      return true;
    }

    // --- Ablesung löschen ---

    public async Task DeleteRecordAsync(ElectricMeterDataRecord record)
    {
      await RunWithBusyAsync(async () =>
      {
        Data!.Records.Remove(record);

        // Nach dem Entfernen Differenzwerte aller verbleibenden Datensätze
        // neu berechnen, da sich die Kette geändert hat
        RecalculateDifferences();

        await _storage!.UpsertElectricMeterDataAsync(Data);

        Data = Data; // Trigger PropertyChanged
      });
    }

    // --- Private Hilfsmethoden ---

    private void BuildNewRecord()
    {
      NewReadingDate = DateTime.Today.ToString("yyyy-MM-dd");
      NewReadingValue = string.Empty;
      NewKilowattHourPrice = _definition?.KilowattHourPrice.ToString("G") ?? "0";
      DateError = string.Empty;
      ValueError = string.Empty;
    }

    private void RefreshRecordList()
    {
      Records.Clear();
      if (_data == null) return;

      // Absteigend nach Datum sortieren, neueste zuerst
      foreach (var r in _data.Records.OrderByDescending(r => r.ReadingDate))
        Records.Add(r);

      // Abgeleitete Properties müssen explizit notifiziert werden
      OnPropertyChanged(nameof(LastRecordDateDisplay));
      OnPropertyChanged(nameof(LastRecordValueDisplay));
      OnPropertyChanged(nameof(AverageAmountDisplay));
      OnPropertyChanged(nameof(AverageValueDisplay));
    }

    private void RecalculateDifferences()
    {
      if (_data == null) return;

      var sorted = _data.Records.OrderBy(r => r.ReadingDate).ToList();
      for (int i = 0; i < sorted.Count; i++)
      {
        if (i == 0)
        {
          sorted[i].DifferenceValue = 0;
          sorted[i].DifferenceDays = 0;
        }
        else
        {
          var prev = sorted[i - 1];
          sorted[i].DifferenceValue = Math.Round(sorted[i].ReadingValue - prev.ReadingValue, 2);
          if (DateTime.TryParse(prev.ReadingDate, out var prevDate) &&
              DateTime.TryParse(sorted[i].ReadingDate, out var currDate))
            sorted[i].DifferenceDays = (currDate - prevDate).Days;
        }
      }
    }
  }
}
