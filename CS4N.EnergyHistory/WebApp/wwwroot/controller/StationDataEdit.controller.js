const localStorageEntry_ViewData = "StationDataEdit-ViewData";
const localStorageEntry_StationDataYear = "StationDataEdit-StationData.Year";

sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector",
  "sap/m/MessageBox",
  "sap/m/MessageToast"],
  function (Controller, Connector, MessageBox, MessageToast) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.StationDataEdit", {

      chartControl: null,

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("StationDataEdit").attachPatternMatched(this.onRouteMatched, this);
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          viewData: {
            stationDefinition: null,
            stationData: null
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

      calculateSums: function () {
        const stationData = this.model.getProperty("/viewData/stationData");

        let total = 0;
        for (let i = 0; i < stationData.years.length; i++) {
          // Jahreswerte summieren
          const yearData = stationData.years[i];
          let yearTotal = 0;

          for (let j = 0; j < yearData.months.length; j++) {
            // Monatswert summieren
            const monthData = yearData.months[j];

            yearTotal += Number(monthData.collectedTotal);
          }

          if (yearData.automaticSummation)
            // Summe der Monatswerte verwenden
            yearData.collectedTotal = Number(yearTotal.toFixed(3)) || 0;
          else
            // Manuelle Eingabe typsicher machen
            yearData.collectedTotal = Number(yearData.collectedTotal) || 0;

          total += Number(yearData.collectedTotal);
        }

        if (stationData.automaticSummation)
          // Summe der Jahreswerte verwenden
          stationData.collectedTotal = total;
        else
          // Manuelle Eingabe typsicher machen
          stationData.collectedTotal = Number(stationData.collectedTotal) || 0;
      },
      // #endregion

      // #region Events
      onRouteMatched: function (evt) {
        this.resetModel();

        const cachedViewData = localStorage.getItem(localStorageEntry_ViewData);
        if (!this.isNullOrEmpty(cachedViewData)) {
          localStorage.removeItem(localStorageEntry_ViewData);

          const viewData = JSON.parse(cachedViewData);

          this.model.setProperty("/viewData", viewData);

          const cachedYearData = localStorage.getItem(localStorageEntry_StationDataYear);
          if (!this.isNullOrEmpty(cachedYearData)) {
            localStorage.removeItem(localStorageEntry_StationDataYear);

            const yearData = JSON.parse(cachedYearData);

            this.model.setProperty(yearData.modelPath, yearData);
          }

          if (viewData.stationData.automaticSummation)
            this.calculateSums();

          this.model.refresh();
          return;
        }

        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.get("StationData/" + evt.getParameters().arguments.guid,
          this.onApiGetViewData.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },

      onBackPress: function () {
        this.navigateTo("StationData", { guid: this.model.getProperty("/viewData/stationDefinition/guid") });
      },

      onYearPress: function (evt) {
        const modelPath = evt.getSource().getBindingContext().getPath(),
          yearData = this.model.getProperty(modelPath);

        yearData.modelPath = modelPath;

        localStorage.setItem(localStorageEntry_ViewData, JSON.stringify(this.model.getProperty("/viewData")));
        localStorage.setItem(localStorageEntry_StationDataYear, JSON.stringify(yearData));

        this.navigateTo("StationDataEditYear", { guid: this.model.getProperty("/viewData/stationDefinition/guid") });
      },

      onSavePress: function () {
        const stationData = this.model.getProperty("/viewData/stationData"),
          container = this.byId("myPage");
        container.setBusy(true);
        Connector.post("StationData", stationData,
          this.onApiPostStationData.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },

      onAddYearPress: function () {
        const yearCollection = this.model.getProperty("/viewData/stationData/years");
        let targetYear = new Date().getFullYear();

        if (yearCollection.some(entry => entry.number === targetYear))
          targetYear = yearCollection.reduce((lowest, obj) => Math.min(obj.number, lowest), Infinity) - 1;

        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.get("StationData/template/" + this.model.getProperty("/viewData/stationDefinition").guid + "/" + targetYear,
          this.onApiGetNewYearTemplate.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },
      // #endregion

      // #region API-Events
      onApiGetViewData: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        this.model.setProperty("/viewData", response);

        if (response.stationData.years.length === 0) {
          Connector.get("StationData/template/" + response.stationDefinition.guid + "/" + new Date().getFullYear(),
            (response) => {
              this.model.getProperty("/viewData/stationData/years").push(response);
              this.model.refresh();
            },
            this.handleApiError.bind(this));
        }
      },

      onApiPostStationData: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        this.model.setProperty("/viewData/stationData", response);
        MessageToast.show(this.i18n.getText("toast_StationDataSaved"));
      },

      onApiGetNewYearTemplate: function (response) {
        const yearCollection = this.model.getProperty("/viewData/stationData/years");

        yearCollection.push(response);
        response.modelPath = "/viewData/stationData/years/" + (yearCollection.length - 1);

        localStorage.setItem(localStorageEntry_ViewData, JSON.stringify(this.model.getProperty("/viewData")));
        localStorage.setItem(localStorageEntry_StationDataYear, JSON.stringify(response));

        this.navigateTo("StationDataEditYear", { guid: this.model.getProperty("/viewData/stationDefinition/guid") });
      }
      // #endregion
    });
  });