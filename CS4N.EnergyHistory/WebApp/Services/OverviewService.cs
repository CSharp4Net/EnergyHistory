using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.WebApp.Models.Overview;
using CS4N.EnergyHistory.WebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CS4N.EnergyHistory.WebApp.Services
{
  internal sealed class OverviewService : ServiceBase
  {
    internal OverviewService(ILogger logger, IDataStore dataStore) : base(logger)
    {
      repository = new StationRepository(dataStore);
    }

    private StationRepository repository;

    internal IActionResult GetItems()
    {
      List<Item> items = [
        new Item("settings", "Stationen", "StationList")
        ];

      items.AddRange(repository.GetStations().Select(station => new Item("stations", station.Name, "Station")).ToList());

      return new OkObjectResult(items);
    }
  }
}