using CS4N.EnergyHistory.Contracts.Models.Data;

namespace CS4N.EnergyHistory.DataStore.File
{
  partial class FileStore
  {
    private static List<StationDataMonth> cachedStationDataOfMonths = [];

    public List<StationDataMonth> GetStationDataOfYear(int stationId, int year)
    {
      var dataOfMonths = cachedStationDataOfMonths.Where(entry => entry.StationId == stationId && entry.Year == year).ToList();
      if (dataOfMonths.Count == 0)
      {
        using var connection = GetConnection();

        dataOfMonths = connection.GetCollection<StationDataMonth>().Find(entry => entry.StationId == stationId && entry.Year == year).ToList();

        cachedStationDataOfMonths.AddRange(dataOfMonths);
      }

      return dataOfMonths;
    }

    public StationDataMonth? GetStationDataOfMonth(int stationId, int year, int month)
    {
      var dataOfMonths = GetStationDataOfYear(stationId, year);

      return dataOfMonths.SingleOrDefault(entry => entry.Number == month);
    }

    public void UpsertStationDataOfMonth(StationDataMonth month)
    {
      using var connection = GetConnection();

      connection.GetCollection<StationDataMonth>().Upsert(month);

      cachedStationDataOfMonths.RemoveAll(entry => entry.StationId == month.StationId && entry.Year == month.Year && entry.Number == month.Number);
    }

    public void DeleteStationDataOfMonth(int stationId, int year, int month)
    {
      using var connection = GetConnection();

      connection.GetCollection<StationDataMonth>()
        .DeleteMany(entry => entry.StationId == stationId && entry.Year == year && entry.Number == month);

      cachedStationDataOfMonths = null;
    }
  }
}