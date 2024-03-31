const localStorageEntry_ViewData = "ElectricMeterDataEdit-ViewData";
const localStorageEntry_ElectricMeterDataYear = "ElectricMeterDataEdit-ElectricMeterData.Year";

sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector",
  "sap/m/MessageBox",
  "sap/m/MessageToast"],
  function (Controller, Connector, MessageBox, MessageToast) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.ElectricMeterDataEdit", {

      chartControl: null,

      importDialogFragment: null,
      importDialog: null,

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("ElectricMeterDataEdit").attachPatternMatched(this.onRouteMatched, this);
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

        const stationGuid = evt.getParameters().arguments.guid,
          cachedViewData = localStorage.getItem(localStorageEntry_ViewData);

        this.model.setProperty("/viewData/definition/guid", stationGuid);

        if (!this.isNullOrEmpty(cachedViewData)) {
          localStorage.removeItem(localStorageEntry_ViewData);

          const viewData = JSON.parse(cachedViewData);

          this.model.setProperty("/viewData", viewData);

          const cachedYearData = localStorage.getItem(localStorageEntry_ElectricMeterDataYear);
          if (!this.isNullOrEmpty(cachedYearData)) {
            localStorage.removeItem(localStorageEntry_ElectricMeterDataYear);

            const yearData = JSON.parse(cachedYearData);

            this.model.setProperty(yearData.modelPath, yearData);
          }

          this.calculateSums();

          this.model.refresh();
          return;
        }

        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.get("ElectricMeterData/" + stationGuid + "/edit",
          this.onApiGetViewData.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },

      onBackPress: function () {
        this.navigateTo("ElectricMeterData", { guid: this.model.getProperty("/viewData/definition/guid") });
      },

      onYearPress: function (evt) {
        const modelPath = evt.getSource().getBindingContext().getPath(),
          yearData = this.model.getProperty(modelPath);

        yearData.modelPath = modelPath;

        localStorage.setItem(localStorageEntry_ViewData, JSON.stringify(this.model.getProperty("/viewData")));
        localStorage.setItem(localStorageEntry_ElectricMeterDataYear, JSON.stringify(yearData));

        this.navigateTo("ElectricMeterDataEditYear", { guid: this.model.getProperty("/viewData/definition/guid") });
      },

      onSavePress: function () {
        const data = this.model.getProperty("/viewData/data"),
          container = this.byId("myPage");
        container.setBusy(true);
        Connector.post("ElectricMeterData/" + data.stationGuid + "/edit", data,
          this.onApiPostElectricMeterData.bind(this),
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
        Connector.get("ElectricMeterData/" + data.stationGuid + "/template/" + targetYear,
          this.onApiGetNewYearTemplate.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },

      onImportPress: function () {
        // create dialog lazily
        this.importDialogFragment ??= this.loadFragment({
          name: "CS4N.EnergyHistory.view.fragment.ElectricMeterDataImportDialog"
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
          Connector.get("ElectricMeterData/" + response.definition.guid + "/template/" + new Date().getFullYear(),
            (response) => {
              this.model.getProperty("/viewData/data/years").push(response);
              this.model.refresh();
            },
            this.handleApiError.bind(this));
        }
      },

      onApiPostElectricMeterData: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        this.model.setProperty("/viewData/data", response);
        MessageToast.show(this.i18n.getText("toast_ElectricMeterDataSaved"));
      },

      onApiGetNewYearTemplate: function (response) {
        const yearCollection = this.model.getProperty("/viewData/data/years");

        yearCollection.push(response);
        response.modelPath = "/viewData/data/years/" + (yearCollection.length - 1);

        localStorage.setItem(localStorageEntry_ViewData, JSON.stringify(this.model.getProperty("/viewData")));
        localStorage.setItem(localStorageEntry_ElectricMeterDataYear, JSON.stringify(response));

        this.navigateTo("ElectricMeterDataEditYear", { guid: this.model.getProperty("/viewData/definition/guid") });
      }
      // #endregion
    });
  });