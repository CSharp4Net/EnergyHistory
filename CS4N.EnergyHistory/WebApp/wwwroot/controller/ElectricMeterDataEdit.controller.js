sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector",
  "sap/m/MessageBox",
  "sap/m/MessageToast"],
  function (Controller, Connector, MessageBox, MessageToast) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.ElectricMeterDataEdit", {

      chartControl: null,
      chartContainer: null,

      filterDialogFragment: null,
      filterDialog: null,

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("ElectricMeterDataEdit").attachPatternMatched(this.onRouteMatched, this);
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          definition: null,
          data: null
        });
      },

      formatPowerValue: function (value) {
        value = Number(value) || 0;

        return value.toLocaleString();
      },

      formatDate: function (value) {
        if (this.isNullOrEmpty(value))
          return value;

        return new Date(value).toLocaleDateString(undefined, { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' });
      },

      convertDateTime: function (value) {
        return value.substring(0, 10);
      },
      // #endregion

      // #region Events
      onRouteMatched: function (evt) {
        this.resetModel();

        const guid = evt.getParameters().arguments.guid,
          container = this.byId("myPage");
        container.setBusy(true);
        Connector.get("ElectricMeterData/data/" + guid,
          this.onApiGetData.bind(this),
          this.handleApiError.bind(this),
          () => this.byId("myPage").setBusy(false));
      },

      onBackPress: function () {
        this.navigateTo("ElectricMeterData");
      },

      onAddRowPress: function () {
        const units = this.model.getProperty("/definition/units");

        for (let i = 0; i < units.length; i++) {
          this.model.getProperty("/data/records").unshift({
            meterUnitCode: units[i].code,
            readingDate: this.convertDateTime(new Date().toISOString()),
            readingDateState: "None",
            value: "",
            valueState: "None",
            editable: true
          });
        }
        this.model.refresh();
      },

      onDeleteRowPress: function (evt) {
        const modelPath = evt.getSource().getBindingContext().getPath(),
          pathElements = modelPath.split("/"),
          arrayIndex = pathElements[pathElements.length - 1];

        this.model.getProperty("/data/records").splice(arrayIndex, 1);
        this.model.refresh();
      },

      onEditRowPress: function (evt) {
        const modelPath = evt.getSource().getBindingContext().getPath();

        this.model.getProperty(modelPath).editable = true;
        this.model.refresh();
      },

      onSavePress: function () {
        const data = this.model.getProperty("/data");

        //let allValid = true;
        for (let i = 0; i < data.records.length; i++) {
          const record = data.records[i];

          record.value = Number(record.value) || 0;
        }

        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.post("ElectricMeterData/data", data,
          this.onApiPostData.bind(this),
          this.handleApiError.bind(this),
          () => this.byId("myPage").setBusy(false));
      },
      // #endregion

      // #region API-Events
      onApiGetData: function (response) {
        response.data.records.map(entry => entry.editable = false);

        this.model.setProperty("/definition", response.definition);
        this.model.setProperty("/data", response.data);
      },

      onApiPostData: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        response.records.map(entry => entry.editable = false);
        this.model.setProperty("/data", response);
        MessageToast.show(this.i18n.getText("toast_ElectricMeterDataSaved"));
      }
      // #endregion
    });
  });