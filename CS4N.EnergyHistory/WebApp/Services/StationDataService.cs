using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.Data;
using CS4N.EnergyHistory.Contracts.Models.Definition;
using CS4N.EnergyHistory.WebApp.Models.Cockpit;
using CS4N.EnergyHistory.WebApp.Models.Station;
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

    internal IActionResult GetKpiValue(string stationId)
    {
      Contracts.Models.Data.StationData data = repository.GetStationData(stationId);

      return new OkObjectResult(new KpiData
      {
        Value = data.CollectedTotal,
        Unit = "Wh"
      });
    }

    internal IActionResult GetStationViewData(string stationId, int year = 0, int month = 0)
    {
      StationDefinition definition = repository.GetStation(stationId)!;

      var result = new ViewData
      {
        StationId = definition.Id,
        StationName = definition.Name,
        StationMaxWattPeak = definition.MaxWattPeak
      };

      StationData data = repository.GetStationData(stationId);

      result.StationCollectedTotal = data.CollectedTotal;

      if (year > 0 && month > 0)
      {
        // Daten aus dem Monat
        var monthData = data.Years.SingleOrDefault(entry => entry.Number == year)?.Months.SingleOrDefault(entry => entry.Number == month);
        if (monthData == null)
        {
          // Jahr existiert noch nicht, leeres Jahr verwenden
          monthData = new StationDataMonth(year, month);
        }

        result.ChartData = monthData.Days
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

        result.ChartData = yearData.Months
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
        result.ChartData = data.Years
          .Select(year => new ChartDataEntry
          {
            X = year.Number.ToString(),
            Y = year.CollectedTotal
          })
          .ToList();
      }

      return new OkObjectResult(result);
    }

    internal IActionResult GetStationDataForEdit(string stationId)
    {
      StationDefinition stationDefinition = repository.GetStation(stationId)!;
      StationData stationData = repository.GetStationData(stationId);

      return new OkObjectResult(new
      {
        stationDefinition,
        stationData
      });
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