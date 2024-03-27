namespace CS4N.EnergyHistory.Contracts
{
  public interface IDataStore
  {
    #region SolarStation
    List<Models.SolarStation.Definition> GetSolarStationDefinitions();
    Models.SolarStation.Definition? GetSolarStationDefinition(string guid);
    void UpsertSolarStationDefinition(Models.SolarStation.Definition definition);
    void DeleteSolarStationDefinition(string guid);

    List<Models.SolarStation.Data.DataSummary> GetSolarStationDatas();
    Models.SolarStation.Data.DataSummary GetSolarStationData(string guid);
    void UpsertSolarStationData(Models.SolarStation.Data.DataSummary data);
    void DeleteSolarStationData(string guid);
    #endregion

    #region ElectricMeter
    List<Models.ElectricMeter.Definition> GetElectricMeterDefinitions();
    Models.ElectricMeter.Definition? GetElectricMeterDefinition(string guid);
    void UpsertElectricMeterDefinition(Models.ElectricMeter.Definition definition);
    void DeleteElectricMeterDefinition(string guid);

    List<Models.ElectricMeter.Data.DataSummary> GetElectricMeterDatas();
    Models.ElectricMeter.Data.DataSummary GetElectricMeterData(string guid);
    void UpsertElectricMeterData(Models.ElectricMeter.Data.DataSummary data);
    void DeleteElectricMeterData(string guid);
    #endregion
  }
}