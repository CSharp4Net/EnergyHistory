sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector",
  "sap/m/MessageBox",
  "sap/m/MessageToast"],
  function (Controller, Connector, MessageBox, MessageToast) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.ElectricMeterData", {

      chartControl: null,
      chartContainer: null,

      filterDialogFragment: null,
      filterDialog: null,

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("ElectricMeterData").attachPatternMatched(this.onRouteMatched, this);
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          guid: "",
          newRecord: null,
          viewData: {
            definiton: null,
            data: {
              records: [],
              originRecords: null
            }
          },
          readingDateState: "None",
          readingValueState: "None",
          compareRecordsActive: false
        });
      },

      convertDateTime: function (value) {
        return value.substring(0, 10);
      },

      formatDate: function (value) {
        if (this.isNullOrEmpty(value))
          return this.i18n.getText("text_NotYetDone");

        return new Date(value).toLocaleDateString(undefined, { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' });
      },

      buildNewRecord() {
        const definition = this.model.getProperty("/viewData/definition");

        this.model.setProperty("/newRecord", {
          readingDate: new Date(),
          readingValue: "",
          kilowattHourPrice: definition.kilowattHourPrice,
          currencyUnit: definition.currencyUnit
        });
      },

      compareRecords: function (records) {
        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.post("ElectricMeterData/" + this.model.getProperty("/guid") + "/compare",
          records,
          this.onApiPostCompareRecords.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },

      restoreRecords: function () {
        this.model.setProperty("/viewData/data/records", JSON.parse(JSON.stringify(this.model.getProperty("/viewData/data/originRecords"))));
        this.model.refresh();
      },
      // #endregion

      // #region Events
      onRouteMatched: function (evt) {
        this.resetModel();
        this.model.setProperty("/guid", evt.getParameters().arguments.guid);

        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.get("ElectricMeterData/" + evt.getParameters().arguments.guid,
          this.onApiGetViewData.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },

      onBackPress: function () {
        this.navigateTo("Cockpit");
      },

      onAddPress: function () {
        const newRecord = this.model.getProperty("/newRecord");

        newRecord.readingValue = Number(newRecord.readingValue) || 0;
        newRecord.kilowattHourPrice = Number(newRecord.kilowattHourPrice) || 0;

        let allValid = true;

        if (!newRecord.readingDate) {
          this.model.setProperty("/readingDateState", "Error");
          allValid = false;
        }
        else
          this.model.setProperty("/readingDateState", "None");

        if (newRecord.readingValue < 0) {
          this.model.setProperty("/readingValueState", "Error");
          allValid = false;
        }
        else
          this.model.setProperty("/readingValueState", "None");

        if (newRecord.kilowattHourPrice < 0) {
          this.model.setProperty("/kilowattHourPriceState", "Error");
          allValid = false;
        }
        else
          this.model.setProperty("/kilowattHourPriceState", "None");

        if (!allValid)
          return;

        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.post("ElectricMeterData/" + this.model.getProperty("/viewData/definition/guid"), newRecord,
          this.onApiPostNewRecord.bind(this),
          this.handleApiError.bind(this),
          () => this.byId("myPage").setBusy(false));
      },

      onDeleteRecordPress: function (evt) {
        const modelPath = evt.getSource().getBindingContext().getPath(),
          modelData = this.model.getProperty(modelPath);

        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.delete("ElectricMeterData/" + this.model.getProperty("/viewData/definition/guid") + "/record", modelData,
          this.onApiDeleteRecord.bind(this),
          this.handleApiError.bind(this),
          () => this.byId("myPage").setBusy(false));
      },

      onRecordSelect: function (evt) {
        const modelPath = evt.getSource().getBindingContext().getPath(),
          recordData = this.model.getProperty(modelPath),
          modelData = this.model.getProperty("/viewData");

        const beforeSelectedRecords = modelData.data.records
          .filter(record => record.selectedIndex > -1)
          .sort((a, b) => (a.selectedIndex > b.selectedIndex ? 1 : -1));

        if (!recordData.selected)
          recordData.selectedIndex = -1;
        else if (beforeSelectedRecords.length > 1) {
          beforeSelectedRecords[0].selectedIndex = 0;
          // Vorherigen Eintrag abwählen
          beforeSelectedRecords[1].selected = false;
          beforeSelectedRecords[1].selectedIndex = -1;
          // Neuen Eintrag auswählen
          recordData.selectedIndex = 1;
        }
        else
          recordData.selectedIndex = beforeSelectedRecords.length;

        const afterSelectedRecords = modelData.data.records
          .filter(record => record.selected)
          .sort(record => record.selectedIndex);

        if (afterSelectedRecords.length > 1) {
          this.compareRecords(afterSelectedRecords);
          this.model.setProperty("/compareRecordsActive", true);
        }
        else if (this.model.getProperty("/compareRecordsActive")) {
          this.restoreRecords();
          this.model.setProperty("/compareRecordsActive", false);
        }

        this.model.refresh();
      },
      // #endregion

      // #region API-Events
      onApiGetViewData: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        response.data.records.map(record => record.selected = false);
        response.data.records.map(record => record.selectedIndex = -1);

        this.model.setProperty("/viewData", response);

        this.buildNewRecord();
        this.setFocus("readingValueInput", 100);
      },

      onApiPostNewRecord: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        this.model.setProperty("/viewData/data", response);

        this.buildNewRecord();
        this.setFocus("readingValueInput", 100);
        MessageToast.show(this.i18n.getText("toast_ElectricMeterDataSaved"));
      },

      onApiDeleteRecord: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        this.model.setProperty("/viewData/data", response);

        MessageToast.show(this.i18n.getText("toast_ElectricMeterDataDeleted"));
      },

      onApiPostCompareRecords: function (response) {
        response.map(record => record.selected = true);

        // Backup erstellen
        this.model.setProperty("/viewData/data/originRecords", JSON.parse(JSON.stringify(this.model.getProperty("/viewData/data/records"))));
        this.model.setProperty("/viewData/data/records", response);
        this.model.refresh();
      }
      // #endregion
    });
  });