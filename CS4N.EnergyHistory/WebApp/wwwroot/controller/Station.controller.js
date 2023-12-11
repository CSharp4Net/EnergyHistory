sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector",
  "sap/m/MessageBox",
  "sap/m/MessageToast"],
  function (Controller, Connector, MessageBox, MessageToast) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.Station", {

      chartControl: null,

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("Station").attachPatternMatched(this.onRouteMatched, this);

        const myChart = this.byId("myChart");
        if (!myChart) {
          var html1 = new sap.ui.core.HTML(this.createId("html1"), {
            content: "<canvas id='myChart' responsive='true'></canvas>"
          });
          this.byId("chartPanel").addContent(html1);
        }
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          pageTitle: "...",
          selectedTimePeriod: "Day",
          selectedYear: new Date().getFullYear(),
          selectedMonth: new Date().getMonth() + 1,
          stationId: 0,
          stationData: null,
          chartPanelIsExpanded: false
        });
      },

      formatPowerValue: function (value) {
        return this.format(this.i18n.getText("text_PowerValue"), value.toLocaleString());
      },

      removeChartControl: function () {
        if (this.chartControl) {
          //this.chartControl.clear();
          this.chartControl.destroy();
        }
      },

      addChartControl: function () {
        const ctx = document.getElementById('myChart');

        this.chartControl = new Chart(ctx, {
          type: 'bar',
          data: {
            labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
            datasets: [{
              label: '# of Votes',
              data: [12, 19, 3, 5, 2, 3],
              borderWidth: 1
            }]
          },
          options: {
            scales: {
              y: {
                beginAtZero: true
              }
            }
          }
        });
      },

      reloadData: function () {
        const selectedTimePeriod = this.model.getProperty("/selectedTimePeriod"),
          selectedYear = this.model.getProperty("/selectedYear"),
          selectedMonth = this.model.getProperty("/selectedMonth"),
          stationId = this.model.getProperty("/stationId");

        const urlPath = selectedTimePeriod === 'Month' ?
          "StationData/" + stationId + "/" + selectedYear + "/" + selectedMonth :
          "StationData/" + stationId + "/" + selectedYear;

        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.get(urlPath,
          this.onApiGetStationData.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },
      // #endregion

      // #region Events
      onRouteMatched: function (evt) {
        this.resetModel();
        this.removeChartControl();

        this.model.setProperty("/stationId", evt.getParameters().arguments.id);
        this.reloadData();
      },

      onBackPress: function () {
        this.navigateTo("Cockpit");
      },

      onEditPress: function () {
        const selectedTimePeriod = this.model.getProperty("/selectedTimePeriod");


      },

      onChartPanelExpand: function (evt) {
        const expanded = evt.getParameter("expand");

        if (expanded)
          setTimeout(() => this.addChartControl(), 100);
        else
          this.removeChartControl();
      },
      // #endregion

      // #region API-Events
      onApiGetStationData: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        this.model.setProperty("/station", response);
        //this.model.setProperty("/pageTitle", this.format(this.i18n.getText("title_Station"), [response.name, this.formatPowerValue(response.maxPowerPeak)]));

        if (this.model.getProperty("/chartPanelIsExpanded"))
          this.addChartControl();
      }
      // #endregion
    });
  });