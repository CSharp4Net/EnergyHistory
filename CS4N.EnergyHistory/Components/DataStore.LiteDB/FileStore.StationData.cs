using CS4N.EnergyHistory.Contracts.Models.Station.Data;

namespace CS4N.EnergyHistory.DataStore.File
{
  partial class FileStore
  {
    private static List<StationData> cachedStationDatas = [];

    public List<StationData> GetStationDatas()
    {
      if (cachedStationDatas.Count > 0)
        return cachedStationDatas;

      var definitions = GetStationDefinitions();

      cachedStationDatas = [];

      foreach (var definition in definitions)
        cachedStationDatas.Add(GetStationData(definition.Guid));

      return cachedStationDatas;
    }

    public StationData GetStationData(string stationGuid)
    {
      var data = cachedStationDatas.SingleOrDefault(entry => entry.StationGuid == stationGuid);
      if (data != null)
        return data;

      data = LoadStationDataFile(stationGuid);

      data ??= new StationData
      {
        StationGuid = stationGuid
      };

      cachedStationDatas.Add(data);

      return data;
    }

    public void UpsertStationData(StationData data)
    {
      WriteStationDataFile(data);

      cachedStationDatas.Clear();
    }

    public void DeleteStationData(string stationGuid)
    {
      DeleteStationDataFile(stationGuid);

      cachedStationDatas.Clear();
    }
  }
}