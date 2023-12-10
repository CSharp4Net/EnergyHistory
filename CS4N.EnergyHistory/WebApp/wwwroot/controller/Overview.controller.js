sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector",
  "sap/ui/model/Filter"],
  function (Controller, Connector, Filter) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.Overview", {

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("Overview").attachPatternMatched(this.onRouteMatched, this);
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          items: [],
          stationCount: 0
        });
      },
      // #endregion

      // #region Events
      onRouteMatched: function () {
        this.resetModel();

        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.get("overview",
          this.onApiGetData.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },

      onTilePress: function () {
        this.navigateTo("StationList");
      },

      onTabSelect: function (evt) {
        const key = evt.getParameter("key");

        this.byId("myTiles").getBinding("items").filter(new Filter("category", "EQ", key));
      },
      // #endregion

      // #region API-Events
      onApiGetData: function (response) {
        this.model.setProperty("/items", response);

        const stations = response.filter(item => item.category === "stations");
        this.model.setProperty("/stationCount", stations.length);

        if (stations.length === 0)
          this.byId("myTabBar").fireSelect({ key: "settings" });
        else
          this.byId("myTabBar").fireSelect({ key: "stations" });
      }
      // #endregion
    });
  });