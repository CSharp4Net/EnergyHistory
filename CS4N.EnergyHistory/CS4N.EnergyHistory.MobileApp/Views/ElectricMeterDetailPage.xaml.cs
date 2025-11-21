using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using CS4N.EnergyHistory.DataStore.File;

namespace CS4N.EnergyHistory.MobileApp.Views
{
  [QueryProperty(nameof(ItemGuid), "guid")]
  public partial class ElectricMeterDetailPage : ContentPage
  {
    private string itemGuid = "";

    public string ItemGuid
    {
      get => itemGuid;
      set
      {
        itemGuid = value;
        _ = LoadAsync(itemGuid);
      }
    }

    public ElectricMeterDetailPage()
    {
      InitializeComponent();
    }

    private async Task LoadAsync(string guid)
    {
      if (string.IsNullOrEmpty(guid))
        return;

      await Task.Run(() =>
      {
        var store = new FileStore();
        var def = store.GetElectricMeterDefinition(guid);
        var data = store.GetElectricMeterData(guid);

        MainThread.BeginInvokeOnMainThread(() =>
        {
          if (def != null)
          {
            nameLabel.Text = string.IsNullOrEmpty(def.Name) ? def.Number : def.Name;
            iconImage.Source = string.IsNullOrEmpty(def.IconUrl) ? null : def.IconUrl;
            numberLabel.Text = $"Number: {def.Number}";
          }

          if (data != null)
          {
            try
            {
              lastValueLabel.Text = $"Last: {Math.Round(data.LastRecordValue)} {def?.CapacityUnit}";
            }
            catch
            {
              lastValueLabel.Text = "Last: -";
            }
          }
        });
      });
    }
  }
}