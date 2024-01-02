const localStorageEntry_ViewData = "StationDataEdit-ViewData";
const localStorageEntry_StationDataYear = "StationDataEdit-StationData.Year";

sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector",
  "sap/m/MessageBox",
  "sap/m/MessageToast"],
  function (Controller, Connector, MessageBox, MessageToast) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.StationDataEditYear", {

      chartControl: null,

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("StationDataEditYear").attachPatternMatched(this.onRouteMatched, this);
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          viewData: {
            stationDefinition: {
              guid: ""
            },
            yearData: null
          },
          collectedTotalState: "None"
        });
      },

      formatPowerValue: function (value, unit) {
        value = Number(value) || 0;

        let textTemplate = this.i18n.getText("text_PowerValue");

        textTemplate = textTemplate.replace("{value}", value.toLocaleString());
        textTemplate = textTemplate.replace("{unit}", unit);

        return textTemplate;
      },

      formatMonth: function (value) {
        switch (value) {
          case 1:
            return this.i18n.getText("text_January");
          case 2:
            return this.i18n.getText("text_February");
          case 3:
            return this.i18n.getText("text_March");
          case 4:
            return this.i18n.getText("text_April");
          case 5:
            return this.i18n.getText("text_May");
          case 6:
            return this.i18n.getText("text_June");
          case 7:
            return this.i18n.getText("text_July");
          case 8:
            return this.i18n.getText("text_August");
          case 9:
            return this.i18n.getText("text_September");
          case 10:
            return this.i18n.getText("text_October");
          case 11:
            return this.i18n.getText("text_November");
          case 12:
            return this.i18n.getText("text_December");
        }
      },
      // #endregion

      // #region Events
      onRouteMatched: function (evt) {
        this.resetModel();
        this.model.setProperty("/viewData/stationDefinition/guid", evt.getParameters().arguments.guid);

        const cachedYearData = localStorage.getItem(localStorageEntry_StationDataYear);
        if (this.isNullOrEmpty(cachedYearData)) {
          this.onBackPress();
          return;
        }
        localStorage.removeItem(localStorageEntry_StationDataYear);

        const cachedViewData = localStorage.getItem(localStorageEntry_ViewData),
          viewData = JSON.parse(cachedViewData),
          yearData = JSON.parse(cachedYearData);

        // 0-Werte für bessere Eingabe umwandeln
        yearData.months.map(entry => {
          if (entry.collectedTotal === 0) entry.collectedTotal = "";
          if (entry.priceOfConsumedKilowattHour === 0) entry.priceOfConsumedKilowattHour = "";
          if (entry.priceOfFedIntoKilowattHour === 0) entry.priceOfFedIntoKilowattHour = "";
        });

        this.model.setProperty("/viewData/stationDefinition", viewData.stationDefinition);
        this.model.setProperty("/viewData/yearData", yearData);
      },

      onBackPress: function () {
        const yearData = this.model.getProperty("/viewData/yearData");

        if (yearData) {
          // 0-Werte wiederherstellen
          yearData.months.map(entry => {
            if (entry.collectedTotal === "")
              entry.collectedTotal = 0;

            if (entry.priceOfConsumedKilowattHour === "")
              entry.priceOfConsumedKilowattHour = 0;
            else
              entry.priceOfConsumedKilowattHour = Number(entry.priceOfConsumedKilowattHour) || 0;

            if (entry.priceOfFedIntoKilowattHour === "")
              entry.priceOfFedIntoKilowattHour = 0;
            else
              entry.priceOfFedIntoKilowattHour = Number(entry.priceOfFedIntoKilowattHour) || 0;
          });

          localStorage.setItem(localStorageEntry_StationDataYear, JSON.stringify(yearData));
        }

        this.navigateTo("StationDataEdit", { guid: this.model.getProperty("/viewData/stationDefinition/guid") });
      },

      onMonthCollectedTotalChange: function () {
        const yearData = this.model.getProperty("/viewData/yearData");
        let yearTotal = 0;

        if (!yearData.automaticSummation)
          return;

        for (let j = 0; j < yearData.months.length; j++) {
          // Monatswert summieren
          const monthData = yearData.months[j];

          // Manuelle Eingabe typsicher machen
          monthData.collectedTotal = Number(monthData.collectedTotal) || 0;

          yearTotal += monthData.collectedTotal;
        }

        if (yearData.manualInput)
          // Manuelle Eingabe typsicher machen
          yearData.collectedTotal = Number(yearData.collectedTotal) || 0;
        else
          // Summe der Monatswerte verwenden
          yearData.collectedTotal = Number(yearTotal.toFixed(3)) || 0;

        this.model.refresh();
      },

      onRemovePress: function () {
        const cachedViewData = localStorage.getItem(localStorageEntry_ViewData),
          viewData = JSON.parse(cachedViewData),
          yearData = this.model.getProperty("/viewData/yearData"),
          pathElements = yearData.modelPath.split("/"),
          arrayIndex = pathElements[pathElements.length - 1];

        viewData.stationData.years.splice(arrayIndex, 1);

        localStorage.setItem(localStorageEntry_ViewData, JSON.stringify(viewData));

        this.navigateTo("StationDataEdit", { guid: this.model.getProperty("/viewData/stationDefinition/guid") });
        MessageToast.show(this.i18n.getText("toast_YearRemoved"));
      },
      // #endregion

      // #region API-Events

      // #endregion
    });
  });