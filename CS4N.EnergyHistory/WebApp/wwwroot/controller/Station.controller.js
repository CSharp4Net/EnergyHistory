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
          this.byId("canvasElementPanel").addContent(html1);
        }
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          pageTitle: ""
        });
      },

      formatPowerValue: function (value) {
        return this.i18n.getText("text_PowerValue").format(value.toLocaleString());
      },

      buildChartControl: function () {
        const ctx = document.getElementById('myChart');

        if (this.chartControl) {
          this.chartControl.clear();
          this.chartControl.destroy();
        }

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
      // #endregion

      // #region Events
      onRouteMatched: function (evt) {
        this.resetModel();

        const id = evt.getParameters().arguments.id,
          container = this.byId("myPage");
        container.setBusy(true);
        Connector.get("StationData/" + id,
          this.onApiGetStation.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },

      onBackPress: function () {
        this.navigateTo("Cockpit");
      },
      // #endregion

      // #region API-Events
      onApiGetStation: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        this.model.setProperty("/station", response);
        this.model.setProperty("/pageTitle", this.i18n.getText("title_Station").format(response.name, this.formatPowerValue(response.maxPowerPeak)));

        this.buildChartControl();
      }
      // #endregion
    });
  });