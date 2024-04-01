using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.ElectricMeter;
using CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data;
using CS4N.EnergyHistory.WebApp.Repositories;
using CS4N.EnergyHistory.WebApp.ViewModels;
using CS4N.EnergyHistory.WebApp.ViewModels.ElectricMeter;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CS4N.EnergyHistory.WebApp.Services
{
  internal sealed class ElectricMeterDataService : ServiceBase
  {
    internal ElectricMeterDataService(ILogger logger, IDataStore dataStore) : base(logger)
    {
      repository = new ElectricMeterRepository(dataStore);
    }

    private ElectricMeterRepository repository;

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

      //viewData.GeneratedElectricityAmount = data.GeneratedElectricityAmount;
      //viewData.GeneratedElectricityValue = data.GeneratedElectricityValue;
      //viewData.FedInElectricityValue = data.FedInElectricityValue;

      DateTime dateFrom = string.IsNullOrEmpty(filter.DateFrom) ? DateTime.MinValue :
         DateTime.ParseExact(filter.DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture);
      DateTime dateTo = string.IsNullOrEmpty(filter.DateTo) ? DateTime.MaxValue :
        DateTime.ParseExact(filter.DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture);

      var recordsInRange = data.Records
        .Where(entry => entry.ReadingDate.Year >= dateFrom.Year && entry.ReadingDate.Year <= dateTo.Year)
        .OrderBy(entry => entry.ReadingDate.Year);

      //switch (filter.StepTypeEnum)
      //{
      //  case ElectricMeterChartDataStepType.Month:
      //    foreach (var year in recordsInRange)
      //    {
      //      if (!year.AutomaticSummation)
      //      {
      //        // Für das Jahr wurde der Gesamtwert manuell erfasst, teile auf temporär auf 12 Monate auf
      //        viewData.ChartData.AddRange(year.Months.Select(month => new ChartDataEntry
      //        {
      //          X = $"{year.Number}-{month.Number:00}",
      //          Y = year.GeneratedElectricityAmount / 12
      //        }).ToList());
      //      }
      //      else if (year.Number == dateFrom.Year && year.Number == dateTo.Year)
      //      {
      //        // Monate ab Anfangsmonat übernehmen
      //        viewData.ChartData.AddRange(year.Months.Where(month => month.Number >= dateFrom.Month && month.Number <= dateTo.Month).Select(month => new ChartDataEntry
      //        {
      //          X = $"{year.Number}-{month.Number:00}",
      //          Y = month.GeneratedElectricityAmount
      //        }));
      //      }
      //      else if (year.Number == dateFrom.Year)
      //      {
      //        // Monate ab Anfangsmonat übernehmen
      //        viewData.ChartData.AddRange(year.Months.Where(month => month.Number >= dateFrom.Month).Select(month => new ChartDataEntry
      //        {
      //          X = $"{year.Number}-{month.Number:00}",
      //          Y = month.GeneratedElectricityAmount
      //        }));
      //      }
      //      else if (year.Number == dateTo.Year)
      //      {
      //        // Monate bis Zielmonat übernehmen
      //        viewData.ChartData.AddRange(year.Months.Where(month => month.Number <= dateTo.Month).Select(month => new ChartDataEntry
      //        {
      //          X = $"{year.Number}-{month.Number:00}",
      //          Y = month.GeneratedElectricityAmount
      //        }));
      //      }
      //      else
      //      {
      //        // Alle Monate übernehmen
      //        viewData.ChartData.AddRange(year.Months.Select(month => new ChartDataEntry
      //        {
      //          X = $"{year.Number}-{month.Number:00}",
      //          Y = month.GeneratedElectricityAmount
      //        }));
      //      }
      //    }
      //    break;

      //  case ElectricMeterChartDataStepType.Year:
      //    viewData.ChartData = recordsInRange.Select(entry => new ChartDataEntry
      //    {
      //      X = entry.ReadingDate.Year.ToString(),
      //      Y = entry.Value
      //    }).ToList();
      //    break;

      //  default:
      //    throw new NotSupportedException($"ChartDataStepType {filter.StepType} not supported!");
      //}

      return new OkObjectResult(viewData);
    }

    internal IActionResult GetSolarStationDataForEdit(object stationGuid)
    {
      throw new NotImplementedException();
    }

    internal IActionResult GetSolarStationDataTemplate(string stationGuid, int year)
    {
      throw new NotImplementedException();
    }

    internal IActionResult GetDataForEdit(string guid)
    {
      throw new NotImplementedException();
    }

    internal IActionResult GetDataTemplate(string guid, int year)
    {
      throw new NotImplementedException();
    }

    internal IActionResult PostDataForEdit(DataSummary data)
    {
      throw new NotImplementedException();
    }
  }
}