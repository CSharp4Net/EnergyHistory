using CS4N.EnergyHistory.Contracts.Models.ElectricMeter;
using CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data;
using CS4N.EnergyHistory.Contracts.Models.SolarStation;
using CS4N.EnergyHistory.Contracts.Models.SolarStation.Data;
using System.Text;
using System.Text.Json;

namespace CS4N.EnergyHistory.MobileApp.Services
{
  /// <summary>
  /// Remote-Implementierung: kommuniziert mit dem bestehenden ASP.NET-Backend
  /// über dessen REST-API. Die Basis-URL wird vom Benutzer in den Einstellungen
  /// festgelegt (z.B. "http://192.168.1.10:5000").
  /// 
  /// Hinweis: Für eine echte SMB-Netzwerkfreigabe wäre eine plattformspezifische
  /// Implementierung notwendig. Der einfachste und plattformübergreifende Weg
  /// ist daher, den WebApp-Server als Proxy zu nutzen, der seinerseits auf die
  /// Freigabe zugreift.
  /// </summary>
  public class RemoteStorageService : IStorageService
  {
    private readonly HttpClient _http;

    // Wiederverwendbare JSON-Optionen (case-insensitives Property-Matching, 
    // da das Backend PascalCase und das JSON ggf. camelCase verwendet)
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
      PropertyNameCaseInsensitive = true
    };

    public RemoteStorageService(string baseUrl)
    {
      _http = new HttpClient
      {
        BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/api/")
      };
    }

    // --- Solar-Stationen ---

    public async Task<List<SolarStationDefinition>> GetSolarStationDefinitionsAsync()
      => await GetAsync<List<SolarStationDefinition>>("SolarStationDefinition") ?? [];

    public async Task<DataSummary> GetSolarStationDataAsync(string guid)
      => await GetAsync<DataSummary>($"SolarStationData/{guid}") ?? new DataSummary { Guid = guid };

    public Task UpsertSolarStationDefinitionAsync(SolarStationDefinition definition)
      => PostAsync("SolarStationDefinition", definition);

    public Task UpsertSolarStationDataAsync(DataSummary data)
      => PostAsync($"SolarStationData/{data.Guid}", data);

    public Task DeleteSolarStationDefinitionAsync(string guid)
      => DeleteAsync($"SolarStationDefinition/{guid}");

    // --- Stromzähler ---

    public async Task<List<ElectricMeterDefinition>> GetElectricMeterDefinitionsAsync()
      => await GetAsync<List<ElectricMeterDefinition>>("ElectricMeterDefinition") ?? [];

    public async Task<ElectricMeterDataObject> GetElectricMeterDataAsync(string guid)
    {
      // Das Backend liefert ein ViewData-Objekt zurück — wir benötigen nur den data-Teil
      var viewData = await GetAsync<ElectricMeterViewDataWrapper>($"ElectricMeterData/{guid}");
      return viewData?.Data ?? new ElectricMeterDataObject { Guid = guid };
    }

    public Task UpsertElectricMeterDefinitionAsync(ElectricMeterDefinition definition)
      => PostAsync("ElectricMeterDefinition", definition);

    public Task UpsertElectricMeterDataAsync(ElectricMeterDataObject data)
      => PostAsync($"ElectricMeterData/{data.Guid}", data);

    public Task DeleteElectricMeterDefinitionAsync(string guid)
      => DeleteAsync($"ElectricMeterDefinition/{guid}");

    // --- Hilfsmethoden ---

    private async Task<T?> GetAsync<T>(string path)
    {
      var response = await _http.GetAsync(path);
      response.EnsureSuccessStatusCode();
      var json = await response.Content.ReadAsStringAsync();
      return JsonSerializer.Deserialize<T>(json, _jsonOptions);
    }

    private async Task PostAsync<T>(string path, T body)
    {
      var json = JsonSerializer.Serialize(body);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      var response = await _http.PostAsync(path, content);
      response.EnsureSuccessStatusCode();
    }

    private async Task DeleteAsync(string path)
    {
      var response = await _http.DeleteAsync(path);
      response.EnsureSuccessStatusCode();
    }

    // Hilfsklasse, um die verschachtelte API-Antwort zu entpacken
    private class ElectricMeterViewDataWrapper
    {
      public ElectricMeterDataObject? Data { get; set; }
    }
  }
}
