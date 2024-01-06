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

      formatPowerValue: function (value) {
        value = Number(value) || 0;

        return value.toLocaleString();
      },

      formatPriceValue: function (power, amount, currency) {
        power = Number(power) || 0;
        amount = Number(amount) || 0;

        let textTemplate = this.i18n.getText("text_PriceValue");

        textTemplate = textTemplate.replace("{amount}", (power * amount).toLocaleString());
        textTemplate = textTemplate.replace("{currency}", currency);

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

            yearTotal += Number(monthData.generatedElectricityAmount);
          }

          if (yearData.automaticSummation)
            // Summe der Monatswerte verwenden
            yearData.generatedElectricityAmount = Number(yearTotal.toFixed(3)) || 0;
          else
            // Manuelle Eingabe typsicher machen
            yearData.generatedElectricityAmount = Number(yearData.generatedElectricityAmount) || 0;

          total += Number(yearData.generatedElectricityAmount);
        }

        // Summe der Jahreswerte verwenden
        stationData.generatedElectricityAmount = total;
      },
      // #endregion

      // #region Events
      onRouteMatched: function (evt) {
        this.resetModel();

        const stationGuid = evt.getParameters().arguments.guid,
          cachedViewData = localStorage.getItem(localStorageEntry_ViewData);

        this.model.setProperty("/viewData/stationDefinition/guid", stationGuid);

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

          this.calculateSums();

          this.model.refresh();
          return;
        }

        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.get("StationData/" + stationGuid + "/edit",
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
        Connector.post("StationData/" + stationData.stationGuid + "/edit", stationData,
          this.onApiPostStationData.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },

      onAddYearPress: function () {
        const stationData = this.model.getProperty("/viewData/stationData");
        let targetYear = new Date().getFullYear();

        if (stationData.years.some(entry => entry.number === targetYear))
          targetYear = stationData.years.reduce((lowest, obj) => Math.min(obj.number, lowest), Infinity) - 1;

        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.get("StationData/" + stationData.stationGuid + "/template/" + targetYear,
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
          // Wurde noch kein Jahr angefügt, dann leeres Jahr abrufen
          Connector.get("StationData/" + response.stationDefinition.guid + "/template/" + new Date().getFullYear(),
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