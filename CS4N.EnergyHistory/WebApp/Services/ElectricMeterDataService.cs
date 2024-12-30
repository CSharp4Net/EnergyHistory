using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.ElectricMeter.Data;
using CS4N.EnergyHistory.WebApp.Repositories;
using CS4N.EnergyHistory.WebApp.ViewModels.ElectricMeter;

namespace CS4N.EnergyHistory.WebApp.Services
{
  internal sealed class ElectricMeterDataService : ServiceBase
  {
    internal ElectricMeterDataService(ILogger logger, IDataStore dataStore) : base(logger)
    {
      repository = new ElectricMeterRepository(dataStore);
    }

    private ElectricMeterRepository repository;

    internal DataView GetData(string guid)
    {
      var definition = repository.GetDefinition(guid)!;
      var dataObject = repository.GetData(guid);

      return new DataView
      {
        Definition = definition,
        Data = dataObject
      };
    }

    internal DataObject PostNewRecord(string guid, DataRecord record)
    {
      var definition = repository.GetDefinition(guid)!;
      var dataObject = repository.GetData(guid);

      record.Id = dataObject.Records.Count > 0 ? dataObject.Records.Max(entry => entry.Id) + 1 : 1;

      dataObject.Records.Add(record);

      // Ablesungen neu komplettieren
      CompleteRecords(dataObject);

      return repository.SetData(dataObject);
    }

    internal object? DeleteRecord(string guid, DataRecord record)
    {
      var definition = repository.GetDefinition(guid)!;
      var dataObject = repository.GetData(guid);

      dataObject.Records.RemoveAll(entry => entry.Id == record.Id);

      // Verbleibende Ablesungen bereinigen
      for (int i = 0; i < dataObject.Records.Count; i++)
      {
        var recordInList = dataObject.Records[i];

        recordInList.DifferenceValue = 0D;
        recordInList.DifferenceDays = 0;
      }

      // Ablesungen neu komplettieren
      CompleteRecords(dataObject);

      return repository.SetData(dataObject);
    }
    private void CompleteRecords(DataObject dataObject)
    {
      var sortedRecords = dataObject.Records.OrderBy(entry => entry.ReadingDate).ToList();

      for (int i = 1; i < sortedRecords.Count; i++)
      {
        var recordInList = sortedRecords[i];
        var recordBefore = sortedRecords[i - 1];

        var recordInListDate = DateTime.Parse(recordInList.ReadingDate);
        var recordBeforeDate = DateTime.Parse(recordBefore.ReadingDate);

        // Zeiten entfernen
        recordInListDate = new DateTime(recordInListDate.Year, recordInListDate.Month, recordInListDate.Day);
        recordBeforeDate = new DateTime(recordBeforeDate.Year, recordBeforeDate.Month, recordBeforeDate.Day);

        recordInList.DifferenceValue = recordInList.ReadingValue - recordBefore.ReadingValue;
        recordInList.DifferenceDays = (recordInListDate - recordBeforeDate).Days;
      }
    }

    internal List<DataRecord> CompareRecords(string guid, DataRecord[] records)
    {
      var sortedRecords = records.OrderBy(entry => entry.ReadingDate).ToList();

      for (int i = 0; i < sortedRecords.Count; i++)
      {
        var recordInList = sortedRecords[i];
        if (i == 0)
        {
          recordInList.DifferenceValue = 0;
          recordInList.DifferenceDays = 0;
          continue;
        }

        var recordBefore = sortedRecords[i - 1];

        var recordInListDate = DateTime.Parse(recordInList.ReadingDate);
        var recordBeforeDate = DateTime.Parse(recordBefore.ReadingDate);

        // Zeiten entfernen
        recordInListDate = new DateTime(recordInListDate.Year, recordInListDate.Month, recordInListDate.Day);
        recordBeforeDate = new DateTime(recordBeforeDate.Year, recordBeforeDate.Month, recordBeforeDate.Day);

        recordInList.DifferenceValue = recordInList.ReadingValue - recordBefore.ReadingValue;
        recordInList.DifferenceDays = (recordInListDate - recordBeforeDate).Days;
      }

      return sortedRecords;
    }
  }
}