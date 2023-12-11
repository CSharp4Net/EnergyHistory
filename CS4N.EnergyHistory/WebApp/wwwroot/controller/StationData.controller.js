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
          pageTitle: "",
          selectedTimePeriod: "Day"
        });
      },

      formatPowerValue: function (value) {
        return this.format(this.i18n.getText("text_PowerValue"), value.toLocaleString());
      },
      // #endregion

      // #region Events
      onRouteMatched: function (evt) {
        this.resetModel();
      },

      onBackPress: function () {
        this.navigateTo("Cockpit");
      },

      onEditPress: function () {
        const selectedTimePeriod = this.model.getProperty("/selectedTimePeriod");


      },
      // #endregion

      // #region API-Events
      onApiGetStation: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }
      }
      // #endregion
    });
  });