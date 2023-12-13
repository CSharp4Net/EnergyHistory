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

    public StationDefinition? GetStationDefinition(string id)
    {
      var definition = GetStationDefinitions()
        .SingleOrDefault(entry => entry.Id.Equals(id));

      return definition;
    }

    public void UpsertStationDefinition(StationDefinition definition)
    {
      var definitions = GetStationDefinitions();

      if (string.IsNullOrEmpty(definition.Id))
      {
        definition.Id = Guid.NewGuid().ToString();
        definitions.Add(definition);
      }
      else
      {
        var existingDefinition = definitions.SingleOrDefault(entry => entry.Id.Equals(definition.Id));

        if (existingDefinition == null)
        {
          definition.Id = Guid.NewGuid().ToString();
          definitions.Add(definition);
        }
        else
        {
          existingDefinition.Name = definition.Name;
          existingDefinition.MaxWattPeak = definition.MaxWattPeak;
          existingDefinition.IconUrl = definition.IconUrl;
          existingDefinition.Location = definition.Location;
          existingDefinition.Modules = definition.Modules;
          existingDefinition.Inverters = definition.Inverters;
        }
      }

      WriteStationDefinitionsFile(definitions);

      cachedStationDefinitions.Clear();
    }

    public void DeleteStationDefinition(string id)
    {
      var definitions = GetStationDefinitions();

      definitions.RemoveAll(entry => entry.Id == id);

      WriteStationDefinitionsFile(definitions);

      cachedStationDefinitions.Clear();
    }
  }
}