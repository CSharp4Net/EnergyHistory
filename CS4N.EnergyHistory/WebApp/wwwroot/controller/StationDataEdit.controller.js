const localStorageEntry_StationDataToEdit = "StationDataToEdit";

sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector",
  "sap/m/MessageBox",
  "sap/m/MessageToast"],
  function (Controller, Connector, MessageBox, MessageToast) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.StationDataEdit", {

      chartControl: null,

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("StationDataEdit").attachPatternMatched(this.onRouteMatched, this);
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          pageTitle: "",
          stationId: "",
          selectedYear: 0,
          selectedMonth: 0,
          stationData: []
        });
      },

      formatPowerValue: function (value) {
        return this.format(this.i18n.getText("text_PowerValue"), value.toLocaleString());
      },
      // #endregion

      // #region Events
      onRouteMatched: function (evt) {
        this.resetModel();

        const cachedData = localStorage.getItem(localStorageEntry_StationDataToEdit);
        if (this.isNullOrEmpty(cachedData)) {
          this.onBackPress();
          return;
        }

        const data = JSON.parse(cachedData);

        this.model.setProperty("/stationId", data.stationId);
        this.model.setProperty("/selectedYear", data.selectedYear);
        this.model.setProperty("/selectedMonth", data.selectedMonth);
      },

      onBackPress: function () {
        this.navigateTo("StationData", { id: this.model.getProperty("/stationId") });
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