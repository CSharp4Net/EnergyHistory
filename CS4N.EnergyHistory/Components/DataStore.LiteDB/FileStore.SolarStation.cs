using CS4N.EnergyHistory.Contracts.Models.SolarStation;
using CS4N.EnergyHistory.Contracts.Models.SolarStation.Data;

namespace CS4N.EnergyHistory.DataStore.File
{
  partial class FileStore
  {
    private const string solarStationDefinitionsFileName = "SolarStations.json";

    private static List<Definition> cachedSolarStationDefinitions = [];
    private static List<DataSummary> cachedSolarStationDatas = [];

    #region Definition
    public List<Definition> GetSolarStationDefinitions()
    {
      if (cachedSolarStationDefinitions.Count > 0)
        return cachedSolarStationDefinitions;

      return cachedSolarStationDefinitions = LoadDefinitionsFile<Definition>(solarStationDefinitionsFileName) ?? [];
    }

    public Definition? GetSolarStationDefinition(string guid)
    {
      var definition = GetSolarStationDefinitions()
        .SingleOrDefault(entry => entry.Guid.Equals(guid));

      return definition;
    }

    public void UpsertSolarStationDefinition(Definition definition)
    {
      var definitions = GetSolarStationDefinitions();

      var savedDefinition = definitions.SingleOrDefault(entry => entry.Guid.Equals(definition.Guid));
      if (savedDefinition == null || string.IsNullOrEmpty(definition.Guid))
      {
        definition.Guid = Guid.NewGuid().ToString();
        definition.CreatedAt = DateTime.Now;
        definitions.Add(definition);
      }
      else
      {
        int savedIndex = definitions.IndexOf(savedDefinition);

        definitions.RemoveAt(savedIndex);

        definition.UpdatedAt = DateTime.Now;

        definitions.Insert(savedIndex, definition);
      }

      WriteDefinitionsFile(definitions, solarStationDefinitionsFileName);

      cachedSolarStationDefinitions.Clear();
    }

    public void DeleteSolarStationDefinition(string guid)
    {
      var definitions = GetSolarStationDefinitions();

      definitions.RemoveAll(entry => entry.Guid == guid);

      WriteDefinitionsFile<Definition>(definitions, solarStationDefinitionsFileName);

      cachedSolarStationDefinitions.Clear();
    }
    #endregion

    #region Data
    public List<DataSummary> GetSolarStationDatas()
    {
      if (cachedSolarStationDatas.Count > 0)
        return cachedSolarStationDatas;

      var definitions = GetSolarStationDefinitions();

      cachedSolarStationDatas = [];

      foreach (var definition in definitions)
        cachedSolarStationDatas.Add(GetSolarStationData(definition.Guid));

      return cachedSolarStationDatas;
    }

    public DataSummary GetSolarStationData(string guid)
    {
      var data = cachedSolarStationDatas.SingleOrDefault(entry => entry.Guid == guid);
      if (data != null)
        return data;

      data = LoadDataFile<DataSummary>(guid);

      data ??= new DataSummary
      {
        Guid = guid
      };

      cachedSolarStationDatas.Add(data);

      return data;
    }

    public void UpsertSolarStationData(DataSummary data)
    {
      WriteDataFile(data);

      cachedSolarStationDatas.Clear();
    }

    public void DeleteSolarStationData(string guid)
    {
      DeleteDataFile<DataSummary>(guid);

      cachedSolarStationDatas.Clear();
    }
    #endregion
  }
}