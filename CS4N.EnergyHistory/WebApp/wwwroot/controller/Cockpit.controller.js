﻿sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector",
  "sap/ui/model/Filter"],
  function (Controller, Connector, Filter) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.Cockpit", {

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("Cockpit").attachPatternMatched(this.onRouteMatched, this);
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          items: [],
          selectedTabKey: "all",
          solarStationCount: 0,
          solarStationAdded: false
        });
      },

      formatTileHeader: function (text) {
        if (text.startsWith("text_"))
          return this.i18n.getText(text);
        else
          return text;
      },

      formatTileFooter: function (text) {
        return this.i18n.getText(text);
      },
      // #endregion

      // #region Events
      onRouteMatched: function () {
        this.resetModel();

        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.get("cockpit",
          this.onApiGetCockpitItems.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },

      onTilePress: function (evt) {
        const modelPath = evt.getSource().getBindingContext().getPath(),
          modelData = this.model.getProperty(modelPath);

        if (this.isNullOrEmpty(modelData.navigationParameterAsJsonText))
          this.navigateTo(modelData.navigationViewName);
        else
          this.navigateTo(modelData.navigationViewName, JSON.parse(modelData.navigationParameterAsJsonText));
      },

      onTabSelect: function (evt) {
        const key = evt.getParameter("key");

        if (key == "all")
          this.byId("myTiles").getBinding("items").filter();
        else
          this.byId("myTiles").getBinding("items").filter(new Filter("category", "EQ", key));
      },
      // #endregion

      // #region API-Events
      onApiGetCockpitItems: function (response) {
        this.model.setProperty("/items", response);

        const solarStations = response.filter(item => item.category === "solarStation");
        this.model.setProperty("/solarStationCount", solarStations.length);
        this.model.setProperty("/solarStationAdded", solarStations.length > 0);

        const electricMeters = response.filter(item => item.category === "electricMeter");
        this.model.setProperty("/electricMeterCount", electricMeters.length);
        this.model.setProperty("/electricMeterAdded", electricMeters.length > 0);

        this.byId("myTiles").getBinding("items").filter();
      }
      // #endregion
    });
  });