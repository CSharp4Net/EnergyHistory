using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.Data;
using System.Globalization;
using System.Text;

namespace CS4N.EnergyHistory.DataImport.FritzBox
{
  public sealed class DataImport : IDataImport
  {
    public List<StationDataYear> ReadCsvFile(string filePath)
    {
      string fileContent = File.ReadAllText(filePath, Encoding.UTF8);

      /* Beispielinhalt eines CSV-Export über 2 Jahre (Stand 2024-02-12)
          sep=;
          Datum/Zeit;Energie;Einheit;Energie in Euro;Einheit;CO2-Ausstoss;Einheit;;Ansicht:;2 Jahre;;Datum;12.02.2024 17-08 Uhr
          Oktober 2023;7,642;kWh;2,60;Euro;4,203;kg CO2
          November 2023;18,551;kWh;6,31;Euro;10,203;kg CO2
          Dezember 2023;12,902;kWh;4,39;Euro;7,096;kg CO2
          Januar 2024;23,573;kWh;8,01;Euro;12,965;kg CO2
          Februar 2024;8,036;kWh;2,73;Euro;4,420;kg CO2
       */

      string[] lines = fileContent.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
      for (int i = 2; i <= lines.Length; i++)
      {
        string[] lineElements = lines[i].Split(";", StringSplitOptions.RemoveEmptyEntries);

        string dateText = lineElements[0];
        string electricityAmount = lineElements[1];
        string electricityCapacityUnit = lineElements[2];

        DateTime dateValue = DateTime.ParseExact(dateText, "MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
      }
    }
  }
}