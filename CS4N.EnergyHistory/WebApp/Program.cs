namespace CS4N.EnergyHistory.WebApp
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      await new ServiceApp().StartAsync(new CancellationToken());
    }
  }
}