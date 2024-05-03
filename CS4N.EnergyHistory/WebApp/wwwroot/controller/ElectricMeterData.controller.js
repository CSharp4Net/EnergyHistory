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
            data: []
          },
          readingDateState: "None",
          readingValueState: "None"
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
      // #endregion

      // #region Events
      onRouteMatched: function (evt) {
        this.resetModel();

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
      // #endregion

      // #region API-Events
      onApiGetViewData: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

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
      }
      // #endregion
    });
  });