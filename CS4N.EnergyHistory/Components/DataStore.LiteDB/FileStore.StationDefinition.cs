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

    public StationDefinition? GetStationDefinition(double id)
    {
      var definition = GetStationDefinitions()
        .SingleOrDefault(entry => entry.Id == id);

      return definition; // JsonSerializer.Deserialize<StationDefinition>(JsonSerializer.Serialize(definition));
    }

    public void UpsertStationDefinition(StationDefinition definition)
    {
      var definitions = GetStationDefinitions();

      if (definition.Id == 0)
      {
        definition.Id = DateTime.Now.Ticks;
        definitions.Add(definition);
      }
      else
      {
        var existingDefinition = definitions.SingleOrDefault(entry => entry.Id == definition.Id);

        if (existingDefinition == null)
        {
          definition.Id = DateTime.Now.Ticks;
          definitions.Add(definition);
        }
        else
        {
          existingDefinition = definition;
        }
      }

      WriteStationDefinitionsFile(definitions);

      cachedStationDefinitions.Clear();
    }

    public void DeleteStationDefinition(double id)
    {
      var definitions = GetStationDefinitions();

      definitions.RemoveAll(entry => entry.Id == id);

      WriteStationDefinitionsFile(definitions);

      cachedStationDefinitions.Clear();
    }
  }
}