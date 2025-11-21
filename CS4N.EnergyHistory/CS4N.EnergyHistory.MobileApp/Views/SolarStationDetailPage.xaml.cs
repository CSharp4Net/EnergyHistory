using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using CS4N.EnergyHistory.DataStore.File;
using System.Collections.ObjectModel;
using CS4N.EnergyHistory.MobileApp.Controls;

namespace CS4N.EnergyHistory.MobileApp.Views
{
  [QueryProperty(nameof(ItemGuid), "guid")]
  public partial class SolarStationDetailPage : ContentPage
  {
    private string itemGuid = "";

    // Collection, die die letzten 12 Monate enthält
    public ObservableCollection<ChartMonthData> MonthData { get; } = new ObservableCollection<ChartMonthData>();

    public string ItemGuid
    {
      get => itemGuid;
      set
      {
        itemGuid = value;
        _ = LoadAsync(itemGuid);
      }
    }

    public SolarStationDetailPage()
    {
      InitializeComponent();
      BindingContext = this;

      // Drawable instanziieren und an GraphicsView binden
      var drawable = new LineChartDrawable
      {
        Data = MonthData,   // direkte Referenz, Drawable liest diese Liste beim Zeichnen
        YUnit = "kWh"
      };
      chartView.Drawable = drawable;
    }

    private async Task LoadAsync(string guid)
    {
      if (string.IsNullOrEmpty(guid))
        return;

      await Task.Run(() =>
      {
        var store = new FileStore();
        var def = store.GetSolarStationDefinition(guid);
        var data = store.GetSolarStationData(guid);

        // Erzeuge Liste der letzten 12 Monate (älteste zuerst)
        var months = Enumerable.Range(0, 12)
          .Select(i => DateTime.Today.AddMonths(-11 + i))
          .Select(d => new { Year = d.Year, Month = d.Month, Label = d.ToString("yyyy-MM") })
          .ToList();

        var values = new List<ChartMonthData>();

        foreach (var m in months)
        {
          double value = 0.0;

          var year = data?.Years.SingleOrDefault(y => y.Number == m.Year);
          if (year != null)
          {
            var monthEntry = year.Months.SingleOrDefault(mm => mm.Number == m.Month);
            if (monthEntry != null)
              value = monthEntry.GeneratedElectricityAmount;
            else if (!year.AutomaticSummation)
              // wenn Jahreswert manuell angegeben, auf Monate verteilen (Option)
              value = year.GeneratedElectricityAmount / 12.0;
          }

          values.Add(new ChartMonthData { Label = m.Label, Value = Math.Round(value, 3) });
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
          // UI-Felder setzen
          if (def != null)
          {
            nameLabel.Text = def.Name;
            iconImage.Source = string.IsNullOrEmpty(def.IconUrl) ? null : def.IconUrl;
            installedLabel.Text = string.IsNullOrEmpty(def.InstalledAt) ? "" : $"Installed: {def.InstalledAt}";
            commentsLabel.Text = def.CommonComments;
          }

          if (data != null)
          {
            kpiLabel.Text = $"Generated: {Math.Round(data.GeneratedElectricityAmount)} {def?.CapacityUnit}";
          }

          // MonthData aktualisieren (Drawable nutzt dieselbe Instanz)
          MonthData.Clear();
          foreach (var e in values)
            MonthData.Add(e);

          // GraphicsView neu zeichnen
          chartView.Invalidate();
        });
      });
    }
  }
}