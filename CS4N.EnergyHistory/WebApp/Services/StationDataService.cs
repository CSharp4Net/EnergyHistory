using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.Data;
using CS4N.EnergyHistory.Contracts.Models.Definition;
using CS4N.EnergyHistory.WebApp.Repositories;
using CS4N.EnergyHistory.WebApp.ViewModels.Station;
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

    internal IActionResult GetInitData()
    {
      return new OkObjectResult(new
      {
        filter = new ChartDataFilter()
      });
    }

    internal IActionResult GetStationViewData(string stationGuid, ChartDataFilter filter)
    {
      StationDefinition definition = repository.GetStationDefinition(stationGuid)!;

      var viewData = new StationDataView
      {
        StationDefinition = definition
      };

      StationData data = repository.GetStationData(stationGuid);

      viewData.StationCollectedTotal = data.CollectedTotal;

      //data.Years.Where(year => year.Number >= filter.DateFrom)

      switch (filter.StepTypeEnum)
      {
        case ViewModels.ChartDataStepType.Day:
          // TODO
          break;
        case ViewModels.ChartDataStepType.Month:

          break;
        case ViewModels.ChartDataStepType.Year:

          break;

        default:
          throw new NotSupportedException($"ChartDataStepType {filter.StepType} not supported!");
      }

      //if (year > 0 && month > 0)
      //{
      //  //// Daten aus dem Monat
      //  //var monthData = data.Years.SingleOrDefault(entry => entry.Number == year)?.Months.SingleOrDefault(entry => entry.Number == month);
      //  //if (monthData == null)
      //  //{
      //  //  // Jahr existiert noch nicht, leeres Jahr verwenden
      //  //  monthData = new StationDataMonth(year, month);
      //  //}

      //  //viewData.ChartData = monthData.Days
      //  //  .Select(month => new ChartDataEntry
      //  //  {
      //  //    X = month.Number.ToString(),
      //  //    Y = month.CollectedTotal
      //  //  })
      //  //  .ToList();
      //}
      //else if (year > 0 && month == 0)
      //{
      //  // Daten aus dem Jahr
      //  var yearData = data.Years.SingleOrDefault(entry => entry.Number == year);
      //  if (yearData == null)
      //  {
      //    // Jahr existiert noch nicht, leeres Jahr verwenden
      //    yearData = new StationDataYear(definition, year);
      //  }

      //  viewData.ChartData = yearData.Months
      //    .Select(month => new ChartDataEntry
      //    {
      //      X = month.Number.ToString(),
      //      Y = month.CollectedTotal
      //    })
      //    .ToList();
      //}
      //else
      //{
      //  // Daten aller Jahre
      //  viewData.ChartData = data.Years
      //    .Select(year => new ChartDataEntry
      //    {
      //      X = year.Number.ToString(),
      //      Y = year.CollectedTotal
      //    })
      //    .ToList();
      //}

      return new OkObjectResult(viewData);
    }

    internal IActionResult GetStationDataForEdit(string stationGuid)
    {
      var viewData = new StationDataEditView
      {
        StationDefinition = repository.GetStationDefinition(stationGuid),
        StationData = repository.GetStationData(stationGuid)
      };

      return new OkObjectResult(viewData);
    }

    internal IActionResult PostStationDataForEdit(StationData stationData)
    {
      return new OkObjectResult(repository.SetStationData(stationData));
    }

    internal IActionResult GetStationDataTemplate(string stationGuid, int year)
    {
      var definition = repository.GetStationDefinition(stationGuid);

      return new OkObjectResult(new StationDataYear(definition, year));
    }
  }
}