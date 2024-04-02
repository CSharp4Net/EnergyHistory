using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models;
using CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data;
using CS4N.EnergyHistory.Core;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace CS4N.EnergyHistory.DataStore.File
{
  public sealed partial class FileStore : IDataStore
  {
    private const string folderName = "FileStore";

    public string StoreFolderPath => Path.Combine(PathHelper.GetWorkPath(), folderName);

    private void WriteDefinitionsFile<T>(List<T> definitions, string fileName)
    {
      if (!Directory.Exists(StoreFolderPath))
        Directory.CreateDirectory(StoreFolderPath);

      string fileContent = JsonSerializer.Serialize(definitions);
      string filePath = Path.Combine(StoreFolderPath, fileName);

      System.IO.File.WriteAllText(filePath, fileContent, Encoding.UTF8);
    }
    private List<T>? LoadDefinitionsFile<T>(string fileName)
    {
      string filePath = Path.Combine(StoreFolderPath, fileName);

      if (!System.IO.File.Exists(filePath))
        return null;

      string fileContent = System.IO.File.ReadAllText(filePath, Encoding.UTF8);

      if (string.IsNullOrWhiteSpace(fileContent))
        return new List<T>();

      return JsonSerializer.Deserialize<List<T>>(fileContent);
    }

    private void WriteDataFile<T>(T data) where T : DataObjectBase
    {
      if (!Directory.Exists(StoreFolderPath))
        Directory.CreateDirectory(StoreFolderPath);

      string fileContent = JsonSerializer.Serialize(data);
      string filePath = Path.Combine(StoreFolderPath, $"{data.Guid}.json");

      System.IO.File.WriteAllText(filePath, fileContent, Encoding.UTF8);
    }
    private T? LoadDataFile<T>(string stationId) where T : DataObjectBase
    {
      string filePath = Path.Combine(StoreFolderPath, $"{stationId}.json");

      if (!System.IO.File.Exists(filePath))
        return default;

      string fileContent = System.IO.File.ReadAllText(filePath, Encoding.UTF8);

      return JsonSerializer.Deserialize<T>(fileContent);
    }
    private void DeleteDataFile<T>(string stationId) where T : DataObjectBase
    {
      string filePath = Path.Combine(StoreFolderPath, $"{stationId}.json");

      if (System.IO.File.Exists(filePath))
        System.IO.File.Delete(filePath);
    }
  }
}