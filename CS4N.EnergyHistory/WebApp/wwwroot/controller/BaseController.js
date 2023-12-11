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

    showResponseError: function (response) {
      MessageBox.error(this.i18n.getText(response.errorMessage), { title: this.i18n.getText(response.errorTitle) });
    },

    setFocus(controlName, delayInMilliseconds) {
      setTimeout(() => this.byId(controlName).focus(), delayInMilliseconds);
    },

    isNullOrEmpty(value) {
      if (value === undefined || value === null)
        return true;
        
      return value.trim() === "";
    },

    format: function (formatText, formatArguments) {
      let args = Array.isArray(formatArguments) ?
        Array.prototype.slice.call(formatArguments, 0) : [formatArguments];
      for (var i = 0; i < args.length; i++) {
        formatText = formatText.replaceAll("{" + i + "}", args[i])
      }
      return formatText;
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