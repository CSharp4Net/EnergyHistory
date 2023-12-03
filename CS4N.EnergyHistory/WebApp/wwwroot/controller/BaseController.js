sap.ui.define([
  "sap/ui/core/mvc/Controller",
  "sap/ui/Device",
  "sap/m/MessageBox"
], function (Controller, Device, MessageBox) {
  "use strict";

  /*
   * Allgemeine Basisklasse für alle Controller in dieser Anwendung. Bietet allgemeine Methoden und ShortCuts.
   */
  return Controller.extend("CS4N.EnergyHistory.controller.BaseController", {

    onInit: function () {
      // Lokalisierung aus globalen Model verfügbar machen
      this.getView().setModel(sap.ui.getCore().getModel("i18n"), "i18n");
      this.i18n = this.getView().getModel("i18n").getResourceBundle();

      this.initController();
    },

    getRoute: function (pattern) {
      return this.getOwnerComponent().getRouter().getRoute(pattern);
    },

    handleApiError: function (error) {
      MessageBox.error((error.responseText) ? error.responseText : error.statusText);
    },

    /**
     * Führt die Navigation über den Router aus.
     * @public
     * @param {string} navToPage Pattern
     * @param {object} navToParameter Parameter
     */
    navigateTo: function (navToPage, navToParameter) {
      this.getOwnerComponent().getRouter().navTo(navToPage, navToParameter);
    }
    // #endregion
  });
});