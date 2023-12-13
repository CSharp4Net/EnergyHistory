using CS4N.EnergyHistory.Contracts.Models.Data;

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
        cachedStationDatas.Add(GetStationData(definition.Id));

      return cachedStationDatas;
    }

    public StationData GetStationData(string stationId)
    {
      var data = cachedStationDatas.SingleOrDefault(entry => entry.StationId == stationId);
      if (data != null)
        return data;

      data = LoadStationDataFile(stationId);

      data ??= new StationData
      {
        StationId = stationId
      };

      cachedStationDatas.Add(data);

      return data;
    }

    public void UpsertStationData(StationData data)
    {
      WriteStationDataFile(data);

      cachedStationDatas.Clear();
    }

    public void DeleteStationData(string stationId)
    {
      DeleteStationDataFile(stationId);

      cachedStationDatas.Clear();
    }
  }
}