namespace CS4N.EnergyHistory.Contracts.Models.SolarStation
{
  public sealed class ApiDefinition
  {
    public string? RootUrl { get; set; }

    public bool LoginMandatory { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }

    public SupportedInverterApiType InverterApiType { get; set; }

    //public ApiDefinitionRequest LoginRequest { get; set; } = new ApiDefinitionRequest();
    //public ApiDefinitionRequest ConnectionTestRequest { get; set; } = new ApiDefinitionRequest();
    //public ApiDefinitionRequest GetCurrentPowerRequest { get; set; } = new ApiDefinitionRequest();
  }
}