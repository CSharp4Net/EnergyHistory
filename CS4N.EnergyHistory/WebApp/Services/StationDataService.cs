using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.Data;
using CS4N.EnergyHistory.Contracts.Models.Definition;
using CS4N.EnergyHistory.WebApp.ViewModels.Cockpit;
using CS4N.EnergyHistory.WebApp.ViewModels.Station;
using CS4N.EnergyHistory.WebApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Services
{
  internal sealed class StationDataService : ServiceBase
  {
    internal StationDataService(ILogger logger, IDataStore dataStore) : base(logger)
    {
      repository = new StationDataRepository(dataStore);
    }

    private StationDataRepository repository;

    internal IActionResult GetStationViewData(string stationGuid, int year = 0, int month = 0)
    {
      StationDefinition definition = repository.GetStation(stationGuid)!;

      var viewData = new StationDataView
      {
        StationDefinition = definition
      };

      StationData data = repository.GetStationData(stationGuid);

      viewData.StationCollectedTotal = data.CollectedTotal;

      if (year > 0 && month > 0)
      {
        // Daten aus dem Monat
        var monthData = data.Years.SingleOrDefault(entry => entry.Number == year)?.Months.SingleOrDefault(entry => entry.Number == month);
        if (monthData == null)
        {
          // Jahr existiert noch nicht, leeres Jahr verwenden
          monthData = new StationDataMonth(year, month);
        }

        viewData.ChartData = monthData.Days
          .Select(month => new ChartDataEntry
          {
            X = month.Number.ToString(),
            Y = month.CollectedTotal
          })
          .ToList();
      }
      else if (year > 0 && month == 0)
      {
        // Daten aus dem Jahr
        var yearData = data.Years.SingleOrDefault(entry => entry.Number == year);
        if (yearData == null)
        {
          // Jahr existiert noch nicht, leeres Jahr verwenden
          yearData = new StationDataYear(year);
        }

        viewData.ChartData = yearData.Months
          .Select(month => new ChartDataEntry
          {
            X = month.Number.ToString(),
            Y = month.CollectedTotal
          })
          .ToList();
      }
      else
      {
        // Daten aller Jahre
        viewData.ChartData = data.Years
          .Select(year => new ChartDataEntry
          {
            X = year.Number.ToString(),
            Y = year.CollectedTotal
          })
          .ToList();
      }

      return new OkObjectResult(viewData);
    }

    internal IActionResult GetStationDataForEdit(string stationGuid)
    {
      var viewData = new StationDataEditView
      {
        StationDefinition = repository.GetStation(stationGuid),
        StationData = repository.GetStationData(stationGuid)
      };

      return new OkObjectResult(viewData);
    }

    internal IActionResult PostStationDataForEdit(StationData stationData)
    {
      return new OkObjectResult(repository.SetStationData(stationData));
    }

    internal IActionResult GetStationDataTemplate(int year)
    {
      return new OkObjectResult(new StationDataYear(year));
    }
  }
}