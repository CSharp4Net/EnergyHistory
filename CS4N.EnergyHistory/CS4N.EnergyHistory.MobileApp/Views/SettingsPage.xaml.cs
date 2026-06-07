using CS4N.EnergyHistory.MobileApp.ViewModels;

namespace CS4N.EnergyHistory.MobileApp.Views
{
  public partial class SettingsPage : ContentPage
  {
    private readonly SettingsViewModel _vm = new();

    public SettingsPage()
    {
      InitializeComponent();
      BindingContext = _vm;
    }

    private void OnSaveClicked(object sender, EventArgs e) => _vm.Save();

    private async void OnBackClicked(object sender, EventArgs e)
      => await Shell.Current.GoToAsync("..");
  }
}
