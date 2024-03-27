sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector"],
  function (Controller, Connector) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.ElectricMeterDefinitionOverview", {

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("ElectricMeterDefinitionOverview").attachPatternMatched(this.onRouteMatched, this);
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          definitions: []
        });
      },
      // #endregion

      // #region Events
      onRouteMatched: function () {
        this.resetModel();

        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.get("ElectricMeterDefinition/overview",
          this.onApiGetOverview.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },

      onBackPress: function () {
        this.navigateTo("Cockpit");
      },

      onAddPress: function () {
        this.navigateTo("ElectricMeterDefinition");
      },

      onStationPress: function (evt) {
        const modelPath = evt.getSource().getBindingContext().getPath(),
          definition = this.model.getProperty(modelPath);

        this.navigateTo("ElectricMeterDefinition", { guid: definition.guid });
      },
      // #endregion

      // #region API-Events
      onApiGetOverview: function (response) {
        this.model.setProperty("/definitions", response);
      }
      // #endregion
    });
  });