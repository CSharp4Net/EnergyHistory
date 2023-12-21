using CS4N.EnergyHistory.Contracts.Models.Definition;
using System.Text.Json;

namespace CS4N.EnergyHistory.DataStore.File
{
  partial class FileStore
  {
    private static List<StationDefinition> cachedStationDefinitions = [];

    public List<StationDefinition> GetStationDefinitions()
    {
      if (cachedStationDefinitions.Count > 0)
        return cachedStationDefinitions;

      return cachedStationDefinitions = LoadStationDefinitionsFile() ?? [];
    }

    public StationDefinition? GetStationDefinition(string guid)
    {
      var definition = GetStationDefinitions()
        .SingleOrDefault(entry => entry.Guid.Equals(guid));

      return definition;
    }

    public void UpsertStationDefinition(StationDefinition definition)
    {
      var definitions = GetStationDefinitions();

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

      WriteStationDefinitionsFile(definitions);

      cachedStationDefinitions.Clear();
    }

    public void DeleteStationDefinition(string guid)
    {
      var definitions = GetStationDefinitions();

      definitions.RemoveAll(entry => entry.Guid == guid);

      WriteStationDefinitionsFile(definitions);

      cachedStationDefinitions.Clear();
    }
  }
}