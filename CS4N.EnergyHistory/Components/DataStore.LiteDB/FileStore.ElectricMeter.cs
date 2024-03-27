using CS4N.EnergyHistory.Contracts.Models.ElectricMeter;
using CS4N.EnergyHistory.Contracts.Models.SolarStation.Data;

namespace CS4N.EnergyHistory.DataStore.File
{
  partial class FileStore
  {
    private const string electricMeterDefinitionsFileName = "ElectricMeters.json";

    private static List<Definition> cachedElectricMeterDefinitions = [];
    private static List<DataSummary> cachedElectricMeterDatas = [];

    #region Definition
    public List<Definition> GetElectricMeterDefinitions()
    {
      if (cachedElectricMeterDefinitions.Count > 0)
        return cachedElectricMeterDefinitions;

      return cachedElectricMeterDefinitions = LoadDefinitionsFile<Definition>(electricMeterDefinitionsFileName) ?? [];
    }

    public Definition? GetElectricMeterDefinition(string guid)
    {
      var definition = GetElectricMeterDefinitions()
        .SingleOrDefault(entry => entry.Guid.Equals(guid));

      return definition;
    }

    public void UpsertElectricMeterDefinition(Definition definition)
    {
      var definitions = GetElectricMeterDefinitions();

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

      WriteDefinitionsFile(definitions, electricMeterDefinitionsFileName);

      cachedElectricMeterDefinitions.Clear();
    }

    public void DeleteElectricMeterDefinition(string guid)
    {
      var definitions = GetElectricMeterDefinitions();

      definitions.RemoveAll(entry => entry.Guid == guid);

      WriteDefinitionsFile(definitions, electricMeterDefinitionsFileName);

      cachedElectricMeterDefinitions.Clear();
    }
    #endregion

    #region Data

    #endregion
  }
}