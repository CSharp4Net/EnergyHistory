sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector"],
  function (Controller, Connector) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.StationList", {

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("StationList").attachPatternMatched(this.onRouteMatched, this);
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          stations: []
        });
      },
      // #endregion

      // #region Events
      onRouteMatched: function () {
        this.resetModel();

        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.get("station/list",
          this.onApiGetList.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },

      onBackPress: function () {
        this.navigateTo("Overview");
      },

      onAddPress: function () {
        this.navigateTo("Station");
      },

      onStationPress: function (evt) {
        
      },
      // #endregion

      // #region API-Events
      onApiGetList: function (response) {
        this.model.setProperty("/stations", response);
      }
      // #endregion
    });
  });