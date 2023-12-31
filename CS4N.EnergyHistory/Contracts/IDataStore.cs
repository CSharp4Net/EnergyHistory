﻿using CS4N.EnergyHistory.Contracts.Models.Data;
using CS4N.EnergyHistory.Contracts.Models.Definition;

namespace CS4N.EnergyHistory.Contracts
{
  public interface IDataStore
  {
    List<StationDefinition> GetStationDefinitions();
    StationDefinition? GetStationDefinition(string guid);
    void UpsertStationDefinition(StationDefinition definition);
    void DeleteStationDefinition(string guid);

    List<StationData> GetStationDatas();
    StationData GetStationData(string stationGuid);
    void UpsertStationData(StationData data);
    void DeleteStationData(string stationGuid);
  }
}