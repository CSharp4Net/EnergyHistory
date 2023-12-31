﻿sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector"],
  function (Controller, Connector) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.StationDefinitionOverview", {

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("StationDefinitionOverview").attachPatternMatched(this.onRouteMatched, this);
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          stations: []
        });
      },

      formatPowerValue: function (value, unit) {
        let textTemplate = this.i18n.getText("text_PowerValue");

        textTemplate = textTemplate.replace("{value}", value.toLocaleString());
        textTemplate = textTemplate.replace("{unit}", unit);

        return textTemplate;
      },
      // #endregion

      // #region Events
      onRouteMatched: function () {
        this.resetModel();

        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.get("StationDefinition/overview",
          this.onApiGetList.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },

      onBackPress: function () {
        this.navigateTo("Cockpit");
      },

      onAddPress: function () {
        this.navigateTo("StationDefinition");
      },

      onStationPress: function (evt) {
        const modelPath = evt.getSource().getBindingContext().getPath(),
          station = this.model.getProperty(modelPath);

        this.navigateTo("StationDefinition", { guid: station.guid });
      },
      // #endregion

      // #region API-Events
      onApiGetList: function (response) {
        this.model.setProperty("/stations", response);
      }
      // #endregion
    });
  });