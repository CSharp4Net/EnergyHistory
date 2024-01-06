sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector",
  "sap/m/MessageBox",
  "sap/m/MessageToast"],
  function (Controller, Connector, MessageBox, MessageToast) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.StationData", {

      chartControl: null,
      chartContainer: null,

      filterDialogFragment: null,
      filterDialog: null,

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
          filter: null,
          stationGuid: "",
          viewData: {
            stationDefinition: null,
            generatedElectricityAmount: 0,
            chartData: []
          }
        });
      },

      formatPowerValue: function (value) {
        value = Number(value) || 0;

        return value.toLocaleString();
      },

      formatDate: function (value) {
        if (this.isNullOrEmpty(value))
          return value;

        return new Date(value).toLocaleDateString();
      },

      createChartControl: function (chartType) {
        const chartContainerId = this.createId("chartControl");
        this.chartContainer = document.getElementById(chartContainerId).getContext('2d');

        if (this.chartControl)
          this.chartControl.destroy();

        this.chartControl = new Chart(this.chartContainer, {
          type: chartType,
          options: {
            maintainAspectRatio: false,
            plugins: {
              legend: {
                display: false
              },
              tooltip: {
                enabled: false
              }
            },
            elements: {
              bar: {
                borderColor: "rgba(0, 100, 217, 1)",
                backgroundColor: "rgba(0, 100, 217, 0.5)"
              }
            },
            scales: {
              y: {
                title: {
                  display: true,
                  text: this.model.getProperty("/viewData/stationDefinition/capacityUnit"),
                  color: "rgba(0, 100, 217, 1)"
                },
                ticks: {
                  display: false,
                  beginAtZero: true
                }
              },
              x: {
                ticks: {                  
                  color: "rgba(0, 100, 217, 1)"
                }
              }
            },
            animation: {
              duration: 1,
              onComplete: function ({ chart }) {
                const ctx = chart.ctx;

                chart.config.data.datasets.forEach((dataset, i) => {
                  const meta = chart.getDatasetMeta(i);

                  meta.data.forEach((bar, index) => {
                    const data = dataset.data[index];
                    if (data.y > 0) {
                      ctx.fillStyle = "rgba(0, 100, 217, 1)";
                      ctx.textAlign = 'center';
                      ctx.textBaseline = 'bottom';
                      ctx.fillText(data.y.toLocaleString(), bar.x, bar.y - 5);
                    }
                  });
                });
              }
            }
          }
        });
      },

      resetChart: function () {
        const chartData = this.model.getProperty("/viewData/chartData");

        this.chartControl.data.datasets = [{
          data: chartData,
          borderWidth: 1
        }];
        this.chartControl.update();
      },

      drawElementValues: function () {
        const ctx = this.chartContainer.ctx;
        ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
        ctx.fillStyle = this.chartControlFontColor;
        ctx.textAlign = "center";
        ctx.textBaseline = "bottom";

        this.chartControl.data.datasets.forEach((dataset, i) => {
          const meta = this.chartControl.controller.getDatasetMeta(i);
          meta.data.forEach((bar, index) =>
            ctx.fillText(dataset.data[index].y.toLocaleString(), bar._model.x, bar._model.y - 5));
        });
      },

      detectDisplayFormat: function (chartDataStepType) {
        return "MM.yyyy";
      },
      // #endregion

      // #region Events
      onRouteMatched: function (evt) {
        this.resetModel();
        this.model.setProperty("/stationGuid", evt.getParameters().arguments.guid);

        this.byId("myPage").setBusy(true);
        Connector.get("StationData/init",
          this.onApiGetInitData.bind(this),
          this.handleApiError.bind(this));
      },

      onBackPress: function () {
        this.navigateTo("Cockpit");
      },

      onEditPress: function () {
        const stationGuid = this.model.getProperty("/viewData/stationDefinition/guid");

        this.navigateTo("StationDataEdit", { guid: stationGuid });
      },

      onFilterPress: function () {
        // create dialog lazily
        this.filterDialogFragment ??= this.loadFragment({
          name: "CS4N.EnergyHistory.view.fragment.StationDataChartFilterDialog"
        });

        this.filterDialogFragment.then((oDialog) => {
          this.filterDialog = oDialog;
          this.filterDialog.open()
        });
      },

      onFilterDialogStepTypeChange: function () {
        const stepType = this.model.getProperty("/filter/stepType");

        switch (stepType) {
          case "Month":
            this.model.setProperty("/filter/dateFormat", this.i18n.getText("format_DateMonthAndYear"));
            break;
          case "Year":
            this.model.setProperty("/filter/dateFormat", this.i18n.getText("format_DateYearOnly"));
            break;
          default: // Days
            this.model.setProperty("/filter/dateFormat", this.i18n.getText("format_Date"));
            break;
        }
      },

      onFilterDialogAbortPress: function () {
        this.filterDialog.close();
      },

      onFilterDialogAbortSubmitPress: function () {
        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.post("StationData/" + this.model.getProperty("/stationGuid"), this.model.getProperty("/filter"),
          this.onApiGetViewData.bind(this),
          this.handleApiError.bind(this),
          () => {
            container.setBusy(false);
            this.filterDialog.close();
          });
      },
      // #endregion

      // #region API-Events
      onApiGetInitData: function (response) {

        response.filter.dateFormat = "MM.yyyy";

        this.model.setProperty("/filter", response.filter);

        Connector.post("StationData/" + this.model.getProperty("/stationGuid"), response.filter,
          this.onApiGetViewData.bind(this),
          this.handleApiError.bind(this),
          () => this.byId("myPage").setBusy(false));
      },

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