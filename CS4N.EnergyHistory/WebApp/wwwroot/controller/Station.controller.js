sap.ui.define(["CS4N/EnergyHistory/controller/BaseController"],
  function (Controller) {
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

        });
      },
      // #endregion

      // #region Events
      onRouteMatched: function () {
        this.resetModel();
      },
      // #endregion

      // #region API-Events
      onButtonPress: function () {
        this.navigateTo("Hello");
      }
      // #endregion
    });
  });