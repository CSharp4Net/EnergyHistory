using CS4N.EnergyHistory.Contracts.Models.SolarStation;

namespace CS4N.EnergyHistory.Contracts
{
  public interface IDataStore
  {
    #region SolarStation
    List<SolarStationDefinition> GetSolarStationDefinitions();
    SolarStationDefinition? GetSolarStationDefinition(string guid);
    void UpsertSolarStationDefinition(SolarStationDefinition definition);
    void DeleteSolarStationDefinition(string guid);

    List<Models.SolarStation.Data.DataSummary> GetSolarStationDatas();
    Models.SolarStation.Data.DataSummary GetSolarStationData(string guid);
    void UpsertSolarStationData(Models.SolarStation.Data.DataSummary data);
    void DeleteSolarStationData(string guid);
    #endregion

    #region ElectricMeter
    List<Models.ElectricMeter.ElectricMeterDefinition> GetElectricMeterDefinitions();
    Models.ElectricMeter.ElectricMeterDefinition? GetElectricMeterDefinition(string guid);
    void UpsertElectricMeterDefinition(Models.ElectricMeter.ElectricMeterDefinition definition);
    void DeleteElectricMeterDefinition(string guid);

    List<Models.ElectricMeter.Data.ElectricMeterDataObject> GetElectricMeterDatas();
    Models.ElectricMeter.Data.ElectricMeterDataObject GetElectricMeterData(string guid);
    void UpsertElectricMeterData(Models.ElectricMeter.Data.ElectricMeterDataObject data);
    void DeleteElectricMeterData(string guid);
    #endregion
  }
}