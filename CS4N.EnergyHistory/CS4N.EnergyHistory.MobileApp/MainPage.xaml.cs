using CS4N.EnergyHistory.DataStore.File;
using System.Collections.ObjectModel;

namespace CS4N.EnergyHistory.MobileApp
{
  public partial class MainPage : ContentPage
  {
    public ObservableCollection<GroupModel> Groups { get; } = [];

    public MainPage()
    {
      InitializeComponent();
      BindingContext = this;
      _ = LoadGroupsAsync();
    }

    private async Task LoadGroupsAsync()
    {
      try
      {
        MainThread.BeginInvokeOnMainThread(() => loadingIndicator.IsVisible = true);

        await Task.Run(() =>
        {
          var store = new FileStore();

          var groupSolarStations = new GroupModel("PV-Stationen");
          var solarStationDefinitions = store.GetSolarStationDefinitions();

          foreach (var solarStationDefinition in solarStationDefinitions)
          {
            var solarStationData = store.GetSolarStationData(solarStationDefinition.Guid);

            var kpi = Math.Round(solarStationData.GeneratedElectricityAmount);
            var kpiUnit = solarStationDefinition.CapacityUnit ?? "";

            var tile = new TileModel
            {
              Category = "solarStation",
              Name = solarStationDefinition.Name,
              IconUrl = solarStationDefinition.IconUrl ?? "",
              FooterText = $"{solarStationData.GeneratedElectricityValue:C2} {solarStationDefinition.CurrencyUnit}",
              KpiText = $"{kpi:N} {kpiUnit}",
              KpiColor = Colors.Green,
              Guid = solarStationDefinition.Guid
            };

            MainThread.BeginInvokeOnMainThread(() => groupSolarStations.Add(tile));
          }

          var electricMeterGroup = new GroupModel("Stromzähler");
          var electricMeterDefinitions = store.GetElectricMeterDefinitions();

          foreach (var electricMeterDefinition in electricMeterDefinitions)
          {
            var data = store.GetElectricMeterData(electricMeterDefinition.Guid);

            double lastValue = 0;
            try { lastValue = data.LastRecordValue; } catch { lastValue = 0; }

            var tile = new TileModel
            {
              Category = "electricMeter",
              Name = string.IsNullOrEmpty(electricMeterDefinition.Name) ? electricMeterDefinition.Number : electricMeterDefinition.Name,
              IconUrl = electricMeterDefinition.IconUrl ?? "",
              FooterText = "ElectricMeter",
              KpiText = $"{Math.Round(lastValue):G} {electricMeterDefinition.CapacityUnit}",
              KpiColor = Colors.Red,
              Guid = electricMeterDefinition.Guid
            };

            MainThread.BeginInvokeOnMainThread(() => electricMeterGroup.Add(tile));
          }

          MainThread.BeginInvokeOnMainThread(() =>
          {
            Groups.Clear();
            if (groupSolarStations.Count > 0) Groups.Add(groupSolarStations);
            if (electricMeterGroup.Count > 0) Groups.Add(electricMeterGroup);
          });
        });
      }
      finally
      {
        MainThread.BeginInvokeOnMainThread(() => loadingIndicator.IsVisible = false);
      }
    }

    private async void GroupedCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.CurrentSelection?.FirstOrDefault() is TileModel tile)
      {
        try
        {
          var route = tile.Category == "solarStation" ? 
            "SolarStation.Detail" : "ElectricMeter.Detail";
          var uri = $"{route}?guid={Uri.EscapeDataString(tile.Guid)}";
          await Shell.Current.GoToAsync(uri);
        }
        catch (Exception ex)
        {
          await DisplayAlert("Navigation Error", ex.Message, "OK");
        }

        ((CollectionView)sender).SelectedItem = null;
      }
    }

    public sealed class TileModel
    {
      public required string Category { get; set; }
      public required string Name { get; set; }
      public string? IconUrl { get; set; }
      public string? FooterText { get; set; }
      public required string KpiText { get; set; }
      public required string Guid { get; set; }
      public required Color KpiColor { get; set; } = Colors.White;
    }

    public sealed class GroupModel : ObservableCollection<TileModel>
    {
      public GroupModel(string key) => Key = key;
      public string Key { get; }
    }
  }
}
