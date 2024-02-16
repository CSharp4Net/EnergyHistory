using CS4N.EnergyHistory.Contracts.Models.Data;

namespace CS4N.EnergyHistory.Contracts
{
  public interface IDataImport
  {
    List<StationDataYear> ReadCsvFile(string filePath);
  }
}