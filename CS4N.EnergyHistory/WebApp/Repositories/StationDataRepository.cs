using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models;
using CS4N.EnergyHistory.Contracts.Models.Data;
using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.WebApp.Repositories
{
  internal sealed class StationDataRepository : RepositoryBase
  {
    internal StationDataRepository(IDataStore dataStore) : base(dataStore) { }

    internal StationDefinition? GetStation(int id)
      => DataStore.GetStationDefinition(id);

    internal StationDataMonth GetStationDataOfMonth(int stationId, int year, int month)
    {
      StationDataMonth? data = DataStore.GetStationDataOfMonth(stationId, year, month);

      data ??= new StationDataMonth
      {
        StationId = stationId,
        Year = year,
        Number = month
      };

      return data;
    }

    internal StationDataYear GetStationDataOfYear(int stationId, int year)
    {
      List<StationDataMonth> collection = DataStore.GetStationDataOfYear(stationId, year);

      return new StationDataYear
      {
        Number = year,
        Months = collection,
        PowerTotal = collection.Sum(data => data.PowerTotal)
      };
    }

    internal ActionReply AddDataOfMonth(StationDataMonth data)
    {
      DataStore.UpsertStationDataOfMonth(data);

      return new ActionReply();
    }

    internal ActionReply UpdateDataOfMonth(StationDataMonth data)
    {
      DataStore.UpsertStationDataOfMonth(data);

      return new ActionReply();
    }

    internal ActionReply DeleteDataOfMonth(StationDataMonth data)
    {
      DataStore.DeleteStationDefinition(data.Id);

      return new ActionReply();
    }
  }
}