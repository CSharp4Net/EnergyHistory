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
        this.model.getProperty("/data/records").unshift({
          readingDate: new Date().toLocaleDateString(undefined, { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' }),
          readingDateState: "None",
          value: "",
          valueState: "None",
          isNew: true
        });
        this.model.refresh();
      },

      onDeleteRowPress: function (evt) {
        const modelPath = evt.getSource().getBindingContext().getPath(),
          pathElements = modelPath.split("/"),
          arrayIndex = pathElements[pathElements.length - 1];

        this.model.getProperty("/data/records").splice(arrayIndex, 1);
        this.model.refresh();
      },

      onSavePress: function () {
        const data = this.model.getProperty("/data");
      },
      // #endregion

      // #region API-Events
      onApiGetData: function (response) {
        this.model.setProperty("/definition", response.definition);
        this.model.setProperty("/data", response.data);

        response.data.records.map(entry => entry.isNew = false);
      }
      // #endregion
    });
  });