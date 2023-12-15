const localStorageEntry_StationDataToEdit = "StationDataToEdit";

sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector",
  "sap/m/MessageBox",
  "sap/m/MessageToast"],
  function (Controller, Connector, MessageBox, MessageToast) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.StationData", {

      chartControl: null,

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("StationData").attachPatternMatched(this.onRouteMatched, this);

        // Füge einen Verweis auf die ChartJS-Library hinzu, welche das MDE ausliefert
        const jsScript = document.createElement("script");
        jsScript.id = "getChartJS";
        jsScript.type = "text/javascript";
        jsScript.src = "./lib/ChartJS/Chart.min.js";
        jsScript.addEventListener("load", () => {
          // Dieser Event-Handler wird einmalig beim Öffnen der App ausgelöst, danach
          // hält der Browser sich Chart.JS und die App im Cache
        });
        document.getElementsByTagName('HEAD')[0].appendChild(jsScript);
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          pageTitle: "...",
          selectedTimePeriod: "Month",
          selectedYear: new Date().getFullYear(),
          selectedMonth: new Date().getMonth() + 1,
          station: {
            stationId: "",
            stationName: "",
            stationMaxWattPeak: 0,
            chartData: null
          }
        });
      },

      formatPowerValue: function (value) {
        if (!value)
          return value;

        return this.format(this.i18n.getText("text_PowerValue"), value.toLocaleString());
      },

      createChartControl: function (chartType) {
        const chartContainerId = this.createId("myChart"),
          chartContainer = document.getElementById(chartContainerId).getContext('2d');

        if (this.myChart)
          this.myChart.destroy();

        this.myChart = new Chart(chartContainer, {
          type: chartType,
          data: {},
          options: {
            animation: { duration: 500 },
            //title: { display: true, text: ":)" },
            //elements: { line: { tension: 0 } },
            legend: { display: false },
            maintainAspectRatio: false,
            responsiveAnimationDuration: 500
          }
        });
      },

      resetChart: function () {
        const chartData = this.model.getProperty("/station/chartData");

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
        const ctx = this.myChart.ctx;
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

      reloadData: function (stationId) {
        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.get("StationData/" + stationId + "/" + this.model.getProperty("/selectedYear") + "/" + this.model.getProperty("/selectedMonth"),
          this.onApiGetStationData.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },
      // #endregion

      // #region Events
      onRouteMatched: function (evt) {
        this.resetModel();

        const cachedData = localStorage.getItem(localStorageEntry_StationDataToEdit);
        if (!this.isNullOrEmpty(cachedData)) {
          localStorage.removeItem(localStorageEntry_StationDataToEdit);

          const data = JSON.parse(cachedData);
          this.model.setProperty("/selectedYear", data.selectedYear);
          this.model.setProperty("/selectedMonth", data.selectedMonth);
        }

        this.reloadData(evt.getParameters().arguments.id);
      },

      onBackPress: function () {
        this.navigateTo("Cockpit");
      },

      onEditPress: function () {
        const stationId = this.model.getProperty("/station/stationId");

        localStorage.setItem(localStorageEntry_StationDataToEdit, JSON.stringify({
          stationId,
          selectedYear: this.model.getProperty("/selectedYear"),
          selectedMonth: this.model.getProperty("/selectedMonth")
        }));

        this.navigateTo("StationDataEdit", { id: stationId });
      },
      // #endregion

      // #region API-Events
      onApiGetStationData: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        this.model.setProperty("/station", response);
        this.model.setProperty("/pageTitle", this.format(this.i18n.getText("title_Station"), response.stationName));

        this.createChartControl("bar");
        this.resetChart();
      }
      // #endregion
    });
  });