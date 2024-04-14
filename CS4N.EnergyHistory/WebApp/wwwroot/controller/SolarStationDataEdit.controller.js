const localStorageEntry_ViewData = "SolarStationDataEdit-ViewData";
const localStorageEntry_SolarStationDataYear = "SolarStationDataEdit-SolarStationData.Year";

sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector",
  "sap/m/MessageBox",
  "sap/m/MessageToast"],
  function (Controller, Connector, MessageBox, MessageToast) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.SolarStationDataEdit", {

      chartControl: null,

      importDialogFragment: null,
      importDialog: null,

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("SolarStationDataEdit").attachPatternMatched(this.onRouteMatched, this);
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          viewData: {
            definition: null,
            data: null
          },
          collectedTotalState: "None",
          importData: {
            filePath: ""
          }
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
        const data = this.model.getProperty("/viewData/data");

        let total = 0;
        for (let i = 0; i < data.years.length; i++) {
          // Jahreswerte summieren
          const yearData = data.years[i];
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
        data.generatedElectricityAmount = total;
      },
      // #endregion

      // #region Events
      onRouteMatched: function (evt) {
        this.resetModel();

        const guid = evt.getParameters().arguments.guid,
          cachedViewData = localStorage.getItem(localStorageEntry_ViewData);

        this.model.setProperty("/viewData/definition/guid", guid);

        if (!this.isNullOrEmpty(cachedViewData)) {
          localStorage.removeItem(localStorageEntry_ViewData);

          const viewData = JSON.parse(cachedViewData);

          this.model.setProperty("/viewData", viewData);

          const cachedYearData = localStorage.getItem(localStorageEntry_SolarStationDataYear);
          if (!this.isNullOrEmpty(cachedYearData)) {
            localStorage.removeItem(localStorageEntry_SolarStationDataYear);

            const yearData = JSON.parse(cachedYearData);

            this.model.setProperty(yearData.modelPath, yearData);
          }

          this.calculateSums();

          this.model.refresh();
          return;
        }

        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.get("SolarStationData/" + guid + "/edit",
          this.onApiGetViewData.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },

      onBackPress: function () {
        this.navigateTo("SolarStationData", { guid: this.model.getProperty("/viewData/definition/guid") });
      },

      onYearPress: function (evt) {
        const modelPath = evt.getSource().getBindingContext().getPath(),
          yearData = this.model.getProperty(modelPath);

        yearData.modelPath = modelPath;

        localStorage.setItem(localStorageEntry_ViewData, JSON.stringify(this.model.getProperty("/viewData")));
        localStorage.setItem(localStorageEntry_SolarStationDataYear, JSON.stringify(yearData));

        this.navigateTo("SolarStationDataEditYear", { guid: this.model.getProperty("/viewData/definition/guid") });
      },

      onSavePress: function () {
        const data = this.model.getProperty("/viewData/data"),
          container = this.byId("myPage");
        container.setBusy(true);
        Connector.post("SolarStationData/" + data.guid + "/edit", data,
          this.onApiPostSolarStationData.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },

      onAddYearPress: function () {
        const data = this.model.getProperty("/viewData/data");
        let targetYear = new Date().getFullYear();

        if (data.years.some(entry => entry.number === targetYear))
          targetYear = data.years.reduce((lowest, obj) => Math.min(obj.number, lowest), Infinity) - 1;

        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.get("SolarStationData/" + data.guid + "/template/" + targetYear,
          this.onApiGetNewYearTemplate.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },

      onImportPress: function () {
        // create dialog lazily
        this.importDialogFragment ??= this.loadFragment({
          name: "CS4N.EnergyHistory.view.fragment.SolarStationDataImportDialog"
        });

        this.importDialogFragment.then((oDialog) => {
          this.importDialog = oDialog;
          this.importDialog.open()
        });
      },

      handleUploadComplete: function (oEvent) {
        var oFileToRead = oEvent.getParameters().files["0"];

        var sResponse = oEvent.getParameter("response"),
          aRegexResult = /\d{4}/.exec(sResponse),
          iHttpStatusCode = aRegexResult && parseInt(aRegexResult[0]),
          sMessage;

        if (sResponse) {
          sMessage = iHttpStatusCode === 200 ? sResponse + " (Upload Success)" : sResponse + " (Upload Error)";
          MessageToast.show(sMessage);
        }
      },
      // #endregion

      // #region API-Events
      onApiGetViewData: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        this.model.setProperty("/viewData", response);

        if (response.data.years.length === 0) {
          // Wurde noch kein Jahr angefügt, dann leeres Jahr abrufen
          Connector.get("SolarStationData/" + response.definition.guid + "/template/" + new Date().getFullYear(),
            (response) => {
              this.model.getProperty("/viewData/data/years").push(response);
              this.model.refresh();
            },
            this.handleApiError.bind(this));
        }
      },

      onApiPostSolarStationData: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        this.model.setProperty("/viewData/data", response);
        MessageToast.show(this.i18n.getText("toast_SolarStationDataSaved"));
      },

      onApiGetNewYearTemplate: function (response) {
        const yearCollection = this.model.getProperty("/viewData/data/years");

        yearCollection.push(response);
        response.modelPath = "/viewData/data/years/" + (yearCollection.length - 1);

        localStorage.setItem(localStorageEntry_ViewData, JSON.stringify(this.model.getProperty("/viewData")));
        localStorage.setItem(localStorageEntry_SolarStationDataYear, JSON.stringify(response));

        this.navigateTo("SolarStationDataEditYear", { guid: this.model.getProperty("/viewData/definition/guid") });
      }
      // #endregion
    });
  });