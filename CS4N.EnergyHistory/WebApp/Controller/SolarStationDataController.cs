﻿using CS4N.EnergyHistory.Contracts;
using CS4N.EnergyHistory.Contracts.Models.SolarStation.Data;
using CS4N.EnergyHistory.WebApp.Services;
using CS4N.EnergyHistory.WebApp.ViewModels.SolarStation;
using Microsoft.AspNetCore.Mvc;

namespace CS4N.EnergyHistory.WebApp.Controller
{
  [Route("api/[controller]")]
  public sealed class SolarStationDataController : ControllerBase
  {
    public SolarStationDataController(ILogger logger, IDataStore dataStore)
    {
      service = new SolarStationDataService(logger, dataStore);
    }

    private SolarStationDataService service;

    [HttpGet("init")]
    public IActionResult GetInitData()
      => service.GetInitData();

    [HttpPost("{guid}")]
    public IActionResult GetData(string guid, [FromBody] ChartDataFilter filter)
      => service.GetData(guid, filter);

    [HttpGet("{guid}/edit")]
    public IActionResult GetDataForEdit(string guid)
      => service.GetDataForEdit(guid);

    [HttpGet("{guid}/template/{year}")]
    public IActionResult GetDataTemplate(string guid, int year)
      => service.GetDataTemplate(guid, year);

    [HttpPost("{guid}/edit")]
    public IActionResult PostDataForEdit([FromBody] DataSummary data)
      => service.PostDataForEdit(data);

    [HttpPost("{guid}/import/fritzbox/csv/years")]
    public IActionResult PostImportDataFromFritzBoxAsCsvOfYears(string guid, [FromForm] object file)
      => service.PostImportDataFromFritzBoxAsCsvOfYears(guid, file);
  }
}