using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.Data;
using CS4N.EnergyHistory.Contracts.Models.Definition;
using CS4N.EnergyHistory.WebApp.Repositories;
using CS4N.EnergyHistory.WebApp.ViewModels;
using CS4N.EnergyHistory.WebApp.ViewModels.Station;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

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

      viewData.GeneratedElectricityAmount = data.GeneratedElectricityAmount;
      viewData.GeneratedElectricityValue = data.GeneratedElectricityValue;
      viewData.FedInElectricityValue = data.FedInElectricityValue;

      DateTime dateFrom = string.IsNullOrEmpty(filter.DateFrom) ? DateTime.MinValue :
         DateTime.ParseExact(filter.DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture);
      DateTime dateTo = string.IsNullOrEmpty(filter.DateTo) ? DateTime.MaxValue :
        DateTime.ParseExact(filter.DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture);

      var yearsInRange = data.Years
        .Where(year => year.Number >= dateFrom.Year && year.Number <= dateTo.Year)
        .OrderBy(year => year.Number);

      switch (filter.StepTypeEnum)
      {
        case ChartDataStepType.Day:
          // TODO : Falls später eine Monatsansicht implementiert wird, wo Erträge pro Tag gelistet werden,
          // muss hier die Datenbeschaffung dafür erfolgen.
          break;

        case ChartDataStepType.Month:
          foreach (var year in yearsInRange)
          {
            if (!year.AutomaticSummation)
            {
              // Für das Jahr wurde der Gesamtwert manuell erfasst, teile auf temporär auf 12 Monate auf
              viewData.ChartData.AddRange(year.Months.Select(month => new ChartDataEntry
              {
                X = $"{year.Number}-{month.Number:00}",
                Y = year.GeneratedElectricityAmount / 12
              }).ToList());
            }
            else if (year.Number == dateFrom.Year && year.Number == dateTo.Year)
            {
              // Monate ab Anfangsmonat übernehmen
              viewData.ChartData.AddRange(year.Months.Where(month => month.Number >= dateFrom.Month && month.Number <= dateTo.Month).Select(month => new ChartDataEntry
              {
                X = $"{year.Number}-{month.Number:00}",
                Y = month.GeneratedElectricityAmount
              }));
            }
            else if (year.Number == dateFrom.Year)
            {
              // Monate ab Anfangsmonat übernehmen
              viewData.ChartData.AddRange(year.Months.Where(month => month.Number >= dateFrom.Month).Select(month => new ChartDataEntry
              {
                X = $"{year.Number}-{month.Number:00}",
                Y = month.GeneratedElectricityAmount
              }));
            }
            else if (year.Number == dateTo.Year)
            {
              // Monate bis Zielmonat übernehmen
              viewData.ChartData.AddRange(year.Months.Where(month => month.Number <= dateTo.Month).Select(month => new ChartDataEntry
              {
                X = $"{year.Number}-{month.Number:00}",
                Y = month.GeneratedElectricityAmount
              }));
            }
            else
            {
              // Alle Monate übernehmen
              viewData.ChartData.AddRange(year.Months.Select(month => new ChartDataEntry
              {
                X = $"{year.Number}-{month.Number:00}",
                Y = month.GeneratedElectricityAmount
              }));
            }
          }
          break;

        case ChartDataStepType.Year:
          viewData.ChartData = yearsInRange.Select(year => new ChartDataEntry
          {
            X = year.Number.ToString(),
            Y = year.GeneratedElectricityAmount
          }).ToList();
          break;

        default:
          throw new NotSupportedException($"ChartDataStepType {filter.StepType} not supported!");
      }

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

    internal IActionResult PostImportDataFromFritzBoxAsCsvOfYears(string stationGuid, string filePath)
    {
      var definition = repository.GetStationDefinition(stationGuid);

      IDataImport dataImport = new DataImport.FritzBox.DataImport(definition);

      var yearsData = dataImport.ReadCsvFile(filePath);


      throw new NotImplementedException();
    }
  }
}