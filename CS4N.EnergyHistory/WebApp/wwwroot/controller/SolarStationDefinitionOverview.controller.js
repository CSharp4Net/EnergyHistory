sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector"],
  function (Controller, Connector) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.SolarStationDefinitionOverview", {

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("SolarStationDefinitionOverview").attachPatternMatched(this.onRouteMatched, this);
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          definitions: []
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
        Connector.get("SolarStationDefinition/overview",
          this.onApiGetList.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },

      onBackPress: function () {
        this.navigateTo("Cockpit");
      },

      onAddPress: function () {
        this.navigateTo("SolarStationDefinition");
      },

      onStationPress: function (evt) {
        const modelPath = evt.getSource().getBindingContext().getPath(),
          definition = this.model.getProperty(modelPath);

        this.navigateTo("SolarStationDefinition", { guid: definition.guid });
      },
      // #endregion

      // #region API-Events
      onApiGetList: function (response) {
        this.model.setProperty("/definitions", response);
      }
      // #endregion
    });
  });