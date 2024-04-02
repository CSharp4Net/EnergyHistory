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

    internal DataView GetData()
    {
      var result = new DataView();

      var definitions = repository.GetDefinitions();
      foreach (var definition in definitions)
      {
        result.Datas.Add(repository.GetData(definition.Guid));
      }

      return result;
    }

    internal ActionReply PostData(DataObject data)
    {
      throw new NotImplementedException();
    }
  }
}