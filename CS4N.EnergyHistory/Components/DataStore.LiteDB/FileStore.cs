using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.Data;
using CS4N.EnergyHistory.Contracts.Models.Definition;
using CS4N.EnergyHistory.Core;

namespace CS4N.EnergyHistory.DataStore.File
{
  public sealed partial class FileStore : IDataStore
  {
    private LiteDB.LiteDatabase GetConnection()
    {
      string filePath = Path.Combine(PathHelper.GetWorkPath(), "FileStore.db");

      return new LiteDB.LiteDatabase(filePath);
    }
  }
}