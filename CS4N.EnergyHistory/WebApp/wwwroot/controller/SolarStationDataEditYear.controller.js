﻿const localStorageEntry_ViewData = "SolarStationDataEdit-ViewData";
const localStorageEntry_SolarStationDataYear = "SolarStationDataEdit-SolarStationData.Year";

sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector",
  "sap/m/MessageBox",
  "sap/m/MessageToast"],
  function (Controller, Connector, MessageBox, MessageToast) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.SolarStationDataEditYear", {

      chartControl: null,

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("SolarStationDataEditYear").attachPatternMatched(this.onRouteMatched, this);
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          viewData: {
            definition: {
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
        this.model.setProperty("/viewData/definition/guid", evt.getParameters().arguments.guid);

        const cachedYearData = localStorage.getItem(localStorageEntry_SolarStationDataYear);
        if (this.isNullOrEmpty(cachedYearData)) {
          this.onBackPress();
          return;
        }
        localStorage.removeItem(localStorageEntry_SolarStationDataYear);

        const cachedViewData = localStorage.getItem(localStorageEntry_ViewData),
          viewData = JSON.parse(cachedViewData),
          yearData = JSON.parse(cachedYearData);

        // 0-Werte für bessere Eingabe umwandeln
        yearData.months.map(entry => {
          if (entry.generatedElectricityAmount === 0) entry.generatedElectricityAmount = "";
          if (entry.generatedElectricityKilowattHourPrice === 0) entry.generatedElectricityKilowattHourPrice = "";
          if (entry.fedInElectricityKilowattHourPrice === 0) entry.fedInElectricityKilowattHourPrice = "";
        });

        this.model.setProperty("/viewData/definition", viewData.definition);
        this.model.setProperty("/viewData/yearData", yearData);
      },

      onBackPress: function () {
        const yearData = this.model.getProperty("/viewData/yearData");

        if (yearData) {
          // 0-Werte wiederherstellen
          yearData.months.map(entry => {
            if (entry.generatedElectricityAmount === "")
              entry.generatedElectricityAmount = 0;

            if (entry.generatedElectricityKilowattHourPrice === "")
              entry.generatedElectricityKilowattHourPrice = 0;
            else
              entry.generatedElectricityKilowattHourPrice = Number(entry.generatedElectricityKilowattHourPrice) || 0;

            if (entry.fedInElectricityKilowattHourPrice === "")
              entry.fedInElectricityKilowattHourPrice = 0;
            else
              entry.fedInElectricityKilowattHourPrice = Number(entry.fedInElectricityKilowattHourPrice) || 0;
          });

          localStorage.setItem(localStorageEntry_SolarStationDataYear, JSON.stringify(yearData));
        }

        this.navigateTo("SolarStationDataEdit", { guid: this.model.getProperty("/viewData/definition/guid") });
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
          monthData.generatedElectricityAmount = Number(monthData.generatedElectricityAmount) || 0;

          yearTotal += monthData.generatedElectricityAmount;
        }

        if (yearData.manualInput)
          // Manuelle Eingabe typsicher machen
          yearData.generatedElectricityAmount = Number(yearData.generatedElectricityAmount) || 0;
        else
          // Summe der Monatswerte verwenden
          yearData.generatedElectricityAmount = Number(yearTotal.toFixed(3)) || 0;

        this.model.refresh();
      },

      onRemovePress: function () {
        const cachedViewData = localStorage.getItem(localStorageEntry_ViewData),
          viewData = JSON.parse(cachedViewData),
          yearData = this.model.getProperty("/viewData/yearData"),
          pathElements = yearData.modelPath.split("/"),
          arrayIndex = pathElements[pathElements.length - 1];

        viewData.data.years.splice(arrayIndex, 1);

        localStorage.setItem(localStorageEntry_ViewData, JSON.stringify(viewData));

        this.navigateTo("SolarStationDataEdit", { guid: this.model.getProperty("/viewData/definition/guid") });
        MessageToast.show(this.i18n.getText("toast_YearRemoved"));
      },
      // #endregion

      // #region API-Events

      // #endregion
    });
  });