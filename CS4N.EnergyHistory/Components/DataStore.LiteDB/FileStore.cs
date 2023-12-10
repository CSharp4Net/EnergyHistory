using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models;
using CS4N.EnergyHistory.Core;

namespace CS4N.EnergyHistory.DataStore.File
{
  public sealed class FileStore : IDataStore
  {
    private static List<Station>? cachedStations = null;

    public List<Station> GetStations()
    {
      if (cachedStations != null)
        return cachedStations;

      using var connection = GetConnection();

      return cachedStations = connection.GetCollection<Station>()
        .FindAll()
        .ToList();
    }

    public Station? GetStation(int id)
    {
      var stations = GetStations();

      return stations.SingleOrDefault(entry => entry.Id == id);
    }

    public void AddStation(Station station)
    {
      using var connection = GetConnection();

      connection.GetCollection<Station>().Upsert(station);

      cachedStations = null;
    }

    private LiteDB.LiteDatabase GetConnection()
    {
      string filePath = Path.Combine(PathHelper.GetWorkPath(), "FileStore.db");

      return new LiteDB.LiteDatabase(filePath);
    }
  }
}