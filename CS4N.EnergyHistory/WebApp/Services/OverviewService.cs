using CS4N.EnergyHistory.WebApp.Models.Overview;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Services
{
  internal sealed class OverviewService : ServiceBase
  {
    internal OverviewService(ILogger logger) : base(logger)
    { 
    
    }

    internal IActionResult GetItems()
    {
      List<Item> items = [
        new Item("settings", "Stations")
        ];

      return new OkObjectResult(items);
    }
  }
}