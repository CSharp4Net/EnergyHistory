sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector",
  "sap/m/MessageBox",
  "sap/m/MessageToast"],
  function (Controller, Connector, MessageBox, MessageToast) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.Station", {

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("Station").attachPatternMatched(this.onRouteMatched, this);
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          station: {
            id: 0,
            name: ""
          }
        });
      },
      // #endregion

      // #region Events
      onRouteMatched: function (evt) {
        this.resetModel();

        // TODO : Load station by id
      },

      onBackPress: function () {
        this.navigateTo("StationList");
      },

      onSavePress: function () {
        const station = this.model.getProperty("/station");

        // TODO : Validate

        const container = this.byId("myPage");
        container.setBusy(true);
        if (station.id === 0)
          Connector.post("station", station,
            this.onApiSaveStation.bind(this),
            this.handleApiError.bind(this),
            () => container.setBusy(false));
      },
      // #endregion

      // #region API-Events
      onApiSaveStation: function (response) {
        if (!response.successful) {
          this.showResponseError(response);
          return;
        }

        this.onBackPress();
        MessageToast.show(this.i18n.getText("toast_StationAdded"));
      }
      // #endregion
    });
  });