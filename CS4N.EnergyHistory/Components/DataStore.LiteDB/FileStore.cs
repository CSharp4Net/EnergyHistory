using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.Data;
using CS4N.EnergyHistory.Contracts.Models.Definition;
using CS4N.EnergyHistory.Core;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CS4N.EnergyHistory.DataStore.File
{
  public sealed partial class FileStore : IDataStore
  {
    public string StoreFolderPath => Path.Combine(PathHelper.GetWorkPath(), "FileStore");

    private void WriteStationDefinitionsFile(List<StationDefinition> definitions)
    {
      if (!Directory.Exists(StoreFolderPath))
        Directory.CreateDirectory(StoreFolderPath);

      string fileContent = JsonSerializer.Serialize(definitions);
      string filePath = Path.Combine(StoreFolderPath, "Stations.json");

      System.IO.File.WriteAllText(filePath, fileContent, Encoding.UTF8);
    }
    private List<StationDefinition>? LoadStationDefinitionsFile()
    {
      string filePath = Path.Combine(StoreFolderPath, "Stations.json");

      if (!System.IO.File.Exists(filePath))
        return null;

      string fileContent = System.IO.File.ReadAllText(filePath, Encoding.UTF8);

      return JsonSerializer.Deserialize<List<StationDefinition>>(fileContent);
    }

    private void WriteStationDataFile(StationData data)
    {
      if (!Directory.Exists(StoreFolderPath))
        Directory.CreateDirectory(StoreFolderPath);

      string fileContent = JsonSerializer.Serialize(data);
      string filePath = Path.Combine(StoreFolderPath, $"{data.StationId}.json");

      System.IO.File.WriteAllText(filePath, fileContent, Encoding.UTF8);
    }
    private StationData? LoadStationDataFile(string stationId)
    {
      string filePath = Path.Combine(StoreFolderPath, $"{stationId}.json");

      if (!System.IO.File.Exists(filePath))
        return null;

      string fileContent = System.IO.File.ReadAllText(filePath, Encoding.UTF8);

      return JsonSerializer.Deserialize<StationData>(fileContent);
    }
    private void DeleteStationDataFile(string stationId)
    {
      string filePath = Path.Combine(StoreFolderPath, $"{stationId}.json");

      if (System.IO.File.Exists(filePath))
        System.IO.File.Delete(filePath);
    }
  }
}