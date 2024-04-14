using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models;
using CS4N.EnergyHistory.Contracts.Models.ElectricMeter;
using CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data;
using CS4N.EnergyHistory.WebApp.Repositories;
using CS4N.EnergyHistory.WebApp.ViewModels;
using CS4N.EnergyHistory.WebApp.ViewModels.ElectricMeter;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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

    internal DataView GetDatas()
    {
      var result = new DataView();

      var definitions = repository.GetDefinitions();
      foreach (var definition in definitions)
      {
        result.Datas.Add(new DataView.DataViewElectricMeter
        {
          Definition = definition,
          Data = repository.GetData(definition.Guid)
        });
      }

      return result;
    }

    internal DataView.DataViewElectricMeter GetData(string guid)
    {
      var definition = repository.GetDefinition(guid);
      var dataObject = repository.GetData(guid);

      return new DataView.DataViewElectricMeter
      {
        Definition = definition,
        Data = dataObject
      };
    }

    internal ActionReply PostData(DataObject data)
    {
      throw new NotImplementedException();
    }
  }
}