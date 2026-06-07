using CS4N.EnergyHistory.Contracts.Models.SolarStation;
using CS4N.EnergyHistory.Contracts.Models.SolarStation.Data;
using CS4N.EnergyHistory.MobileApp.Services;
using System.Collections.ObjectModel;

namespace CS4N.EnergyHistory.MobileApp.ViewModels
{
  /// <summary>
  /// ViewModel für die Solar-Stations-Detailseite.
  /// Entspricht dem SolarStationData.controller.js aus der WebApp, ergänzt um
  /// die Jahres-/Monatsbearbeitung aus SolarStationDataEdit.controller.js.
  /// </summary>
  public class SolarStationViewModel : BaseViewModel
  {
    private IStorageService? _storage;

    // --- Stammdaten ---

    private SolarStationDefinition? _definition;
    public SolarStationDefinition? Definition
    {
      get => _definition;
      set { SetProperty(ref _definition, value); RefreshSummary(); }
    }

    private DataSummary? _data;
    public DataSummary? Data
    {
      get => _data;
      set { SetProperty(ref _data, value); RefreshSummary(); RefreshYears(); }
    }

    // --- Zusammenfassung (Summen über alle Jahre) ---

    public string GeneratedAmountDisplay =>
      $"{_data?.GeneratedElectricityAmount:N1} {_definition?.CapacityUnit}";

    public string GeneratedValueDisplay =>
      _data?.GeneratedElectricityValue > 0
        ? $"Ertragswert: {_data.GeneratedElectricityValue:C2}"
        : string.Empty;

    public string FedInValueDisplay =>
      _data?.FedInElectricityValue > 0
        ? $"Einspeisevergütung: {_data.FedInElectricityValue:C2}"
        : string.Empty;

    public string InstalledAtDisplay =>
      _definition?.InstalledAt is { Length: > 0 } d
        ? $"In Betrieb seit: {DateTime.Parse(d):dd.MM.yyyy}"
        : string.Empty;

    public string AgeInDaysDisplay
    {
      get
      {
        if (_definition?.InstalledAt is not { Length: > 0 } d) return string.Empty;
        var days = (DateTime.Today - DateTime.Parse(d)).Days;
        return $"Alter: {days} Tage";
      }
    }

    public string AmountPerDayDisplay
    {
      get
      {
        if (_definition?.InstalledAt is not { Length: > 0 } d) return string.Empty;
        var days = (DateTime.Today - DateTime.Parse(d)).Days;
        if (days == 0 || _data == null) return string.Empty;
        var perDay = Math.Round(_data.GeneratedElectricityAmount / days, 2);
        return $"⌀ {perDay:N2} {_definition?.CapacityUnit}/Tag";
      }
    }

    // --- Jahres-Übersicht für die Erfassungs-Liste ---

    public ObservableCollection<YearRowModel> Years { get; } = [];

    // --- Laden ---

    public async Task LoadAsync(string guid)
    {
      await RunWithBusyAsync(async () =>
      {
        _storage = StorageServiceFactory.Create();
        var definitions = await _storage.GetSolarStationDefinitionsAsync();
        Definition = definitions.FirstOrDefault(d => d.Guid == guid);
        Data = await _storage.GetSolarStationDataAsync(guid);
      });
    }

    // --- Jahres-Wert speichern ---

    public async Task SaveYearAsync(YearRowModel yearRow)
    {
      await RunWithBusyAsync(async () =>
      {
        if (_data == null) return;

        var existingYear = _data.Years.FirstOrDefault(y => y.Number == yearRow.Year);
        if (existingYear == null)
        {
          // Neues Jahr anlegen, wenn es noch nicht existiert
          var def = _definition!;
          existingYear = new DataOfYear(def, yearRow.Year);
          _data.Years.Add(existingYear);
        }

        // Werte aus dem Eingabemodell übernehmen
        existingYear.GeneratedElectricityAmount = yearRow.GeneratedAmount;

        // Monatswerte synchronisieren
        for (int i = 0; i < 12; i++)
        {
          var monthRow = yearRow.Months[i];
          var existingMonth = existingYear.Months[i];
          existingMonth.GeneratedElectricityAmount = monthRow.GeneratedAmount;
          existingMonth.FedInElectricityKilowattHourPrice = monthRow.FedInPrice;
          existingMonth.GeneratedElectricityKilowattHourPrice = monthRow.GeneratedPrice;
          existingMonth.Comments = monthRow.Comments;
        }

        await _storage!.UpsertSolarStationDataAsync(_data);

        // UI neu aufbauen
        Data = _data;
      });
    }

    // --- Private Hilfsmethoden ---

    private void RefreshSummary()
    {
      OnPropertyChanged(nameof(GeneratedAmountDisplay));
      OnPropertyChanged(nameof(GeneratedValueDisplay));
      OnPropertyChanged(nameof(FedInValueDisplay));
      OnPropertyChanged(nameof(InstalledAtDisplay));
      OnPropertyChanged(nameof(AgeInDaysDisplay));
      OnPropertyChanged(nameof(AmountPerDayDisplay));
    }

    private void RefreshYears()
    {
      Years.Clear();
      if (_data == null || _definition == null) return;

      // Alle erfassten Jahre anzeigen, neuestes zuerst
      var sorted = _data.Years.OrderByDescending(y => y.Number);
      foreach (var year in sorted)
        Years.Add(new YearRowModel(year, _definition));

      // Wenn noch keine Daten vorhanden, aktuelles Jahr vorschlagen
      if (!Years.Any())
      {
        var newRow = new YearRowModel(DateTime.Today.Year, _definition);
        Years.Add(newRow);
      }
    }
  }

  // --- Datenmodelle für die Solar-Jahreserfassung ---

  /// <summary>
  /// UI-Modell für eine Jahreszeile in der Solar-Erfassung.
  /// Kapselt die bearbeitbaren Felder, die direkt an Eingabefelder gebunden sind.
  /// </summary>
  public class YearRowModel : BaseViewModel
  {
    public int Year { get; }

    private double _generatedAmount;
    public double GeneratedAmount
    {
      get => _generatedAmount;
      set => SetProperty(ref _generatedAmount, value);
    }

    public List<MonthRowModel> Months { get; }

    // Konstruktor für ein bereits gespeichertes Jahr
    public YearRowModel(DataOfYear year, SolarStationDefinition definition)
    {
      Year = year.Number;
      _generatedAmount = year.GeneratedElectricityAmount;
      Months = year.Months.Select(m => new MonthRowModel(m)).ToList();
    }

    // Konstruktor für ein neues Jahr (nur Jahreszahl bekannt)
    public YearRowModel(int year, SolarStationDefinition definition)
    {
      Year = year;
      _generatedAmount = 0;
      Months = Enumerable.Range(1, 12)
        .Select(m => new MonthRowModel(new DataOfMonth(definition, year, m)))
        .ToList();
    }
  }

  public class MonthRowModel : BaseViewModel
  {
    public int Number { get; }
    public string MonthName => new DateTime(2000, Number, 1).ToString("MMMM");

    private double _generatedAmount;
    public double GeneratedAmount
    {
      get => _generatedAmount;
      set => SetProperty(ref _generatedAmount, value);
    }

    private decimal _generatedPrice;
    public decimal GeneratedPrice
    {
      get => _generatedPrice;
      set => SetProperty(ref _generatedPrice, value);
    }

    private decimal _fedInPrice;
    public decimal FedInPrice
    {
      get => _fedInPrice;
      set => SetProperty(ref _fedInPrice, value);
    }

    private string _comments = string.Empty;
    public string Comments
    {
      get => _comments;
      set => SetProperty(ref _comments, value);
    }

    public MonthRowModel(DataOfMonth month)
    {
      Number = month.Number;
      _generatedAmount = month.GeneratedElectricityAmount;
      _generatedPrice = month.GeneratedElectricityKilowattHourPrice;
      _fedInPrice = month.FedInElectricityKilowattHourPrice;
      _comments = month.Comments;
    }
  }
}
