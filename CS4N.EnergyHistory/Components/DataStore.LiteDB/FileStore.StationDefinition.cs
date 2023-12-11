using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.DataStore.File
{
  partial class FileStore
  {
    private static List<StationDefinition> cachedStationDefinitions = [];

    public List<StationDefinition> GetStationDefinitions()
    {
      if (cachedStationDefinitions.Any())
        return cachedStationDefinitions;

      using var connection = GetConnection();

      return cachedStationDefinitions = connection.GetCollection<StationDefinition>()
        .FindAll()
        .ToList();
    }

    public StationDefinition? GetStationDefinition(int id)
    {
      var definitions = GetStationDefinitions();

      return definitions.SingleOrDefault(entry => entry.Id == id);
    }

    public void UpsertStationDefinition(StationDefinition definition)
    {
      using var connection = GetConnection();

      connection.GetCollection<StationDefinition>().Upsert(definition);

      cachedStationDefinitions.Clear();
    }

    public void DeleteStationDefinition(int id)
    {
      using var connection = GetConnection();

      connection.GetCollection<StationDefinition>().Delete(id);

      cachedStationDefinitions.Clear();
    }
  }
}