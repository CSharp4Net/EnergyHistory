using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.Data;
using CS4N.EnergyHistory.Contracts.Models.Definition;
using CS4N.EnergyHistory.Core;

namespace CS4N.EnergyHistory.DataStore.File
{
  public sealed partial class FileStore : IDataStore
  {
    public string StoreFolderPath => Path.Combine(PathHelper.GetWorkPath(), "FileStore");

    private const string _fileNameTemplateStationDefinition = "{stationId}.json";
    private const string _fileNameTemplateYearData = "{stationId}-{year}.json";
    private const string _fileNameTemplateMonthData = "{stationId}-{year}-{month}.json";

    private void WriteStationDefinitionFile(StationDefinition data)
    { 
    
    }
  }
}