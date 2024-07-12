//using System.Net;

//namespace CS4N.EnergyHistory.Contracts.Models.SolarStation
//{
//  public sealed class ApiDefinitionRequest
//  {
//    /// <summary>
//    /// Zusätzliche Werte, welche an die <see cref="RootUrl"/> angehangen werden, die sich darausergebende URL kann zum Abrufen eines Verbindungstest genutzt werden.
//    /// </summary>
//    public string? UrlPattern { get; set; }
//    /// <summary>
//    /// Erwarteter StatusCode der Antwort im Erfolgsfall, alle anderen werden als Fehler interpretiert.
//    /// </summary>
//    public HttpStatusCode SuccessStatusCode { get; set; }
//    /// <summary>
//    /// Erwarteter Pfad zur Eigenschaft innerhalb einer JSON-Antwort im Erfolgsfall.
//    /// </summary>
//    public string? SuccessResultExpectedJsonPath { get; set; }
//    /// <summary>
//    /// Erwarteter Wert der Eigenschaft unter <see cref="SuccessResultExpectedJsonPath"/> innerhalb einer JSON-Antwort im Erfolgsfall.
//    /// </summary>
//    public string? SuccessResultExpectedPropertyValue { get; set; }
//    /// <summary>
//    /// Erwarteter Pfad zur Eigenschaft innerhalb einer JSON-Antwort im Fehlerfall.
//    /// </summary>
//    public string? ErrorResultExpectedJsonPath { get; set; }
//  }
//}