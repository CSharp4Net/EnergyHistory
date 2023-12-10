using CS4N.EnergyHistory.Contracts.Models;

namespace CS4N.EnergyHistory.Contracts
{
  public interface IDataStore
  {
    List<Station> GetStations();

    Station? GetStation(int id);

    void UpsertStation(Station station);
    void DeleteStation(int id);
  }
}