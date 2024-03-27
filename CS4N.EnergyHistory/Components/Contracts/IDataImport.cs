using CS4N.EnergyHistory.Contracts.Models.SolarStation.Data;

namespace CS4N.EnergyHistory.Contracts
{
  public interface IDataImport
  {
    List<DataOfYear> ReadCsvFile(string filePath);
  }
}