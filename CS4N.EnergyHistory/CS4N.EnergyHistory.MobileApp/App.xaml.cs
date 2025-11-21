using Microsoft.Extensions.DependencyInjection;

namespace CS4N.EnergyHistory.MobileApp
{
  public partial class App : Application
  {
    public App()
    {
      InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
      const int newHeight = 667;
      const int newWidth = 375;

      var newWindow = new Window(new AppShell())
      {
        Height = newHeight,
        Width = newWidth
      };

      return newWindow;
    }
  }
}