sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector",
  "sap/m/MessageBox",
  "sap/m/MessageToast"],
  function (Controller, Connector, MessageBox, MessageToast) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.ElectricMeterDefinition", {

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("ElectricMeterDefinition").attachPatternMatched(this.onRouteMatched, this);
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          newRecord: true,
          definition: null,
          commonPropertiesAreValid: "Default",
          nameState: "None",
          numberState: "None"
        });
      },

      validateInput: function (definition) {
        let allValid = true;

        if (this.isNullOrEmpty(definition.number)) {
          this.model.setProperty("/numberState", "Error");
          allValid = false;
        }
        else
          this.model.setProperty("/numberState", "None");

        if (this.isNullOrEmpty(definition.installedAt))
          definition.installedAt = null;

        if (allValid)
          this.model.setProperty("/commonPropertiesAreValid", "Default");
        else
          this.model.setProperty("/commonPropertiesAreValid", "Negative");

        this.model.refresh();

        return allValid;
      },

      formatDateTime: function (value) {
        if (this.isNullOrEmpty(value))
          return value;

        return new Date(value).toLocaleString();
      },
      // #endregion

      // #region Events
      onRouteMatched: function (evt) {
        this.resetModel();

        const container = this.byId("myPage"),
          guid = evt.getParameters().arguments.guid;

        container.setBusy(true);
        if (!guid) {
          Connector.get("ElectricMeterDefinition/new",
            this.onApiGetNewElectricMeter.bind(this),
            this.handleApiError.bind(this),
            () => container.setBusy(false));
        }
        else {
          Connector.get("ElectricMeterDefinition/" + guid,
            this.onApiGetElectricMeter.bind(this),
            this.handleApiError.bind(this),
            () => container.setBusy(false));
        }
      },

      onBackPress: function () {
        this.navigateTo("ElectricMeterDefinitionOverview");
      },

      onSavePress: function () {
        const definition = this.model.getProperty("/definition");

        if (!this.validateInput(definition))
          return;

        definition.kilowattHourPrice = Number(definition.kilowattHourPrice) || 0;

        const container = this.byId("myPage");
        container.setBusy(true);
        if (this.isNullOrEmpty(definition.guid))
          Connector.post("ElectricMeterDefinition", definition,
            this.onApiAddElectricMeter.bind(this),
            this.handleApiError.bind(this),
            () => container.setBusy(false));
        else
          Connector.patch("ElectricMeterDefinition", definition,
            this.onApiUpdateElectricMeter.bind(this),
            this.handleApiError.bind(this),
            () => container.setBusy(false));
      },

      onDeletePress: function () {
        MessageBox.confirm(this.i18n.getText("message_ConfirmDeleteElectricMeter"), {
          actions: [MessageBox.Action.YES, MessageBox.Action.NO],
          emphasizedAction: MessageBox.Action.NO,
          onClose: (evt) => {
            if (evt != "YES")
              return;

            const definition = this.model.getProperty("/definition");

            const container = this.byId("myPage");
            container.setBusy(true);
            Connector.delete("ElectricMeterDefinition", definition,
              this.onApiDeleteElectricMeter.bind(this),
              this.handleApiError.bind(this),
              () => container.setBusy(false));
          }
        });
      },

      onIconExplorerPress: function () {
        window.open("https://ui5.sap.com/test-resources/sap/m/demokit/iconExplorer/webapp/index.html#/overview/SAP-icons", '_blank').focus();
      },

      AddMeterUnitPress: function () {
        this.model.getProperty("/definition/units").push({
          code: "",
          codeState: "None",
          isConsumptionMeter: true,
          kilowattHourPrice: 0
        });
        this.model.refresh();
      },

      DeleteMeterUnitPress: function (evt) {
        const modelPath = evt.getSource().getBindingContext().getPath(),
          pathElements = modelPath.split("/"),
          arrayIndex = pathElements[pathElements.length - 1];

        this.model.getProperty("/definition/units").splice(arrayIndex, 1);
        this.model.refresh();
      },
      // #endregion

      // #region API-Events
      onApiGetNewElectricMeter: function (response) {
        this.model.setProperty("/newRecord", true);
        this.model.setProperty("/definition", response);

        this.setFocus("numberInput", 100);
      },

      onApiAddElectricMeter: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        this.onBackPress();
        MessageToast.show(this.i18n.getText("toast_ElectricMeterAdded"));
      },

      onApiUpdateElectricMeter: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        MessageToast.show(this.i18n.getText("toast_ElectricMeterUpdated"));
      },

      onApiGetElectricMeter: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        this.model.setProperty("/newRecord", false);
        this.model.setProperty("/definition", response);
      },

      onApiDeleteElectricMeter: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        this.onBackPress();
        MessageToast.show(this.i18n.getText("toast_ElectricMeterDeleted"));
      }
      // #endregion
    });
  });