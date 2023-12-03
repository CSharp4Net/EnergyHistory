using System.Diagnostics;

namespace CS4N.EnergyHistory.Core
{
  public static class EnvironmentHelper
  {
    /// <summary>
    /// Gibt an, ob die Anwendung als Consolen-Anwendung ausgeführt wird.
    /// </summary>
    public static bool IsConsoleApplication
    {
      get
      {
        using (Stream stream = Console.OpenStandardInput(1))
        {
          return stream != Stream.Null;
        }
      }
    }

    /// <summary>
    /// Gibt an, ob die Anwendung im Debug-Modus ausgeführt wird.
    /// </summary>
    public static bool IsDebug
    {
      get => Debugger.IsAttached;
    }
  }
}