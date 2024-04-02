using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.SolarStation;
using CS4N.EnergyHistory.Contracts.Models.SolarStation.Data;
using CS4N.EnergyHistory.WebApp.Repositories;
using CS4N.EnergyHistory.WebApp.ViewModels;
using CS4N.EnergyHistory.WebApp.ViewModels.SolarStation;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CS4N.EnergyHistory.WebApp.Services
{
  internal sealed class SolarStationDataService : ServiceBase
  {
    internal SolarStationDataService(ILogger logger, IDataStore dataStore) : base(logger)
    {
      repository = new SolarStationRepository(dataStore);
    }

    private SolarStationRepository repository;

    internal IActionResult GetInitData()
    {
      return new OkObjectResult(new
      {
        filter = new ChartDataFilter()
      });
    }

    internal IActionResult GetData(string guid, ChartDataFilter filter)
    {
      Definition definition = repository.GetDefinition(guid)!;

      var viewData = new DataView
      {
        Definition = definition
      };

      DataSummary data = repository.GetData(guid);

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
        case SolarStationChartDataStepType.Day:
          // TODO : Falls später eine Monatsansicht implementiert wird, wo Erträge pro Tag gelistet werden,
          // muss hier die Datenbeschaffung dafür erfolgen.
          break;

        case SolarStationChartDataStepType.Month:
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

        case SolarStationChartDataStepType.Year:
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

    internal IActionResult GetDataForEdit(string guid)
    {
      var viewData = new DataEditView
      {
        Definition = repository.GetDefinition(guid),
        Data = repository.GetData(guid)
      };

      return new OkObjectResult(viewData);
    }

    internal IActionResult PostDataForEdit(DataSummary data)
    {
      return new OkObjectResult(repository.SetData(data));
    }

    internal IActionResult GetDataTemplate(string guid, int year)
    {
      var definition = repository.GetDefinition(guid);

      return new OkObjectResult(new DataOfYear(definition, year));
    }

    internal IActionResult PostImportDataFromFritzBoxAsCsvOfYears(string guid, object filePath)
    {
      var definition = repository.GetDefinition(guid);

      IDataImport dataImport = new DataImport.FritzBox.DataImport(definition);

      var yearsData = dataImport.ReadCsvFile(filePath.ToString());

      // TODO

      throw new NotImplementedException();
    }
  }
}