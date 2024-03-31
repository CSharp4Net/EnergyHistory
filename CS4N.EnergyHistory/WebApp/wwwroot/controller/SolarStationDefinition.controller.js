sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector",
  "sap/m/MessageBox",
  "sap/m/MessageToast"],
  function (Controller, Connector, MessageBox, MessageToast) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.SolarStationDefinition", {

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("SolarStationDefinition").attachPatternMatched(this.onRouteMatched, this);
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          newRecord: true,
          definition: {
            guid: "",
            name: "",
            powerPeak: "",
            powerUnit: "W",
            capacityUnit: "kW",
            installedAt: "",
            currencyUnit: "",
            purchaseCosts: 0,
            generatedElectricityEnabled: false,
            generatedElectricityKilowattHourPrice: 0,
            fedInEnabled: false,
            fedInElectricityKilowattHourPrice: 0
          },
          commonPropertiesAreValid: "Default",
          nameState: "None",
          powerPeakState: "None",
          validValues: null
        });
      },

      validateInput: function (definition) {
        let allValid = true;

        if (this.isNullOrEmpty(definition.name)) {
          this.model.setProperty("/nameState", "Error");
          allValid = false;
        }
        else
          this.model.setProperty("/nameState", "None");

        definition.powerPeak = Number(definition.powerPeak) || 0;

        if (definition.powerPeak <= 0) {
          this.model.setProperty("/powerPeakState", "Error");
          allValid = false;
        }
        else
          this.model.setProperty("/powerPeakState", "None");

        if (this.isNullOrEmpty(definition.installedAt))
          definition.installedAt = null;

        definition.purchaseCosts = Number(definition.purchaseCosts) || 0;

        if (definition.purchaseCosts < 0)
          definition.purchaseCosts = 0;

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

        const guid = evt.getParameters().arguments.guid;
        if (!guid) {
          this.setFocus("nameInput", 100);
          return;
        }

        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.get("SolarStationDefinition/" + guid,
          this.onApiGetStation.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },

      onBackPress: function () {
        this.navigateTo("SolarStationDefinitionOverview");
      },

      onSavePress: function () {
        const definition = this.model.getProperty("/definition");

        if (!this.validateInput(definition))
          return;

        const container = this.byId("myPage");
        container.setBusy(true);
        if (this.isNullOrEmpty(definition.guid))
          Connector.post("SolarStationDefinition", definition,
            this.onApiAddStation.bind(this),
            this.handleApiError.bind(this),
            () => container.setBusy(false));
        else
          Connector.patch("SolarStationDefinition", definition,
            this.onApiUpdateStation.bind(this),
            this.handleApiError.bind(this),
            () => container.setBusy(false));
      },

      onDeletePress: function () {
        MessageBox.confirm(this.i18n.getText("message_ConfirmDeleteStation"), {
          actions: [MessageBox.Action.YES, MessageBox.Action.NO],
          emphasizedAction: MessageBox.Action.YES,
          onClose: (evt) => {
            if (evt != "YES")
              return;

            const definition = this.model.getProperty("/definition"),
              container = this.byId("myPage");
            container.setBusy(true);
            Connector.delete("SolarStationDefinition", definition,
              this.onApiDeleteStation.bind(this),
              this.handleApiError.bind(this),
              () => container.setBusy(false));
          }
        });
      },

      onIconExplorerPress: function () {
        window.open("https://ui5.sap.com/test-resources/sap/m/demokit/iconExplorer/webapp/index.html#/overview/SAP-icons", '_blank').focus();
      },
      // #endregion

      // #region API-Events
      onApiAddStation: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        this.onBackPress();
        MessageToast.show(this.i18n.getText("toast_StationAdded"));
      },

      onApiUpdateStation: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        MessageToast.show(this.i18n.getText("toast_StationUpdated"));
      },

      onApiGetStation: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        this.model.setProperty("/newRecord", false);
        this.model.setProperty("/definition", response);
      },

      onApiDeleteStation: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        this.onBackPress();
        MessageToast.show(this.i18n.getText("toast_StationDeleted"));
      }
      // #endregion
    });
  });