sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector",
  "sap/m/MessageBox",
  "sap/m/MessageToast"],
  function (Controller, Connector, MessageBox, MessageToast) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.StationData", {

      myChart: null,
      chartContainer: null,

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("StationData").attachPatternMatched(this.onRouteMatched, this);

        //// Füge einen Verweis auf die ChartJS-Library hinzu
        //const jsScript = document.createElement("script");
        //jsScript.id = "getChartJS";
        //jsScript.type = "text/javascript";
        //jsScript.src = "./lib/ChartJS/Chart.min.js";
        //jsScript.addEventListener("load", () => {
        //  // Dieser Event-Handler wird einmalig beim Öffnen der App ausgelöst, danach
        //  // hält der Browser sich Chart.JS und die App im Cache
        //});
        //document.getElementsByTagName('HEAD')[0].appendChild(jsScript);
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          selectedTimePeriod: "Month",
          selectedYear: new Date().getFullYear(),
          selectedMonth: 0,
          viewData: {
            stationDefinition: null,
            stationCollectedTotal: 0,
            chartData: []
          }
        });
      },

      formatPowerValue: function (value, unit) {
        value = Number(value) || 0;

        let textTemplate = this.i18n.getText("text_PowerValue");

        textTemplate = textTemplate.replace("{value}", value.toLocaleString());
        textTemplate = textTemplate.replace("{unit}", unit);

        return textTemplate;
      },

      formatDate: function (value) {
        if (this.isNullOrEmpty(value))
          return value;

        return new Date(value).toLocaleDateString();
      },

      createChartControl: function (chartType) {
        const chartContainerId = this.createId("myChart");
        this.chartContainer = document.getElementById(chartContainerId).getContext('2d');

        if (this.myChart)
          this.myChart.destroy();

        this.myChart = new Chart(this.chartContainer, {
          type: chartType,
          data: {},
          options: {
            animation: {
              duration: 500,
              //onComplete: this.drawElementValues.bind(this)
            },
            title: { display: true, text: "Hallo Welt" },
            //elements: { line: { tension: 0 } },
            //legend: { display: false },
            maintainAspectRatio: false,
            responsiveAnimationDuration: 500
          }
        });
      },

      resetChart: function () {
        const chartData = this.model.getProperty("/viewData/chartData");

        this.myChart.data.labels = chartData.map(entry => entry.x);
        this.myChart.data.datasets = [{
          data: chartData,
          borderWidth: 1,
          backgroundColor: this.myChartElementColor,
          borderColor: this.myChartElementBorderColor
        }];
        this.myChart.update();
      },

      drawElementValues: function () {
        const ctx = this.chartContainer.ctx;
        ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
        ctx.fillStyle = this.myChartFontColor;
        ctx.textAlign = "center";
        ctx.textBaseline = "bottom";

        this.myChart.data.datasets.forEach((dataset, i) => {
          const meta = this.myChart.controller.getDatasetMeta(i);
          meta.data.forEach((bar, index) =>
            ctx.fillText(dataset.data[index].y.toLocaleString(), bar._model.x, bar._model.y - 5));
        });
      },

      reloadData: function (stationGuid) {
        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.get("StationData/" + stationGuid + "/" + this.model.getProperty("/selectedYear") + "/" + this.model.getProperty("/selectedMonth"),
          this.onApiGetViewData.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },
      // #endregion

      // #region Events
      onRouteMatched: function (evt) {
        this.resetModel();
        
        this.reloadData(evt.getParameters().arguments.guid);
      },

      onBackPress: function () {
        this.navigateTo("Cockpit");
      },

      onEditPress: function () {
        const stationGuid = this.model.getProperty("/viewData/stationDefinition/guid");

        this.navigateTo("StationDataEdit", { guid: stationGuid });
      },
      // #endregion

      // #region API-Events
      onApiGetViewData: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        this.model.setProperty("/viewData", response);

        this.createChartControl("bar");
        this.resetChart();
      }
      // #endregion
    });
  });