namespace CS4N.EnergyHistory.WebApp.Services
{
  internal abstract class ServiceBase
  {
    internal ServiceBase(ILogger logger)
    {
      this.logger = logger;
    }

    protected readonly ILogger logger;
  }
}