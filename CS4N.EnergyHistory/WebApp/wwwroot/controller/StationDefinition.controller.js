sap.ui.define([
  "CS4N/EnergyHistory/controller/BaseController",
  "CS4N/EnergyHistory/connector/ApiConnector",
  "sap/m/MessageBox",
  "sap/m/MessageToast"],
  function (Controller, Connector, MessageBox, MessageToast) {
    "use strict";

    return Controller.extend("CS4N.EnergyHistory.controller.StationDefinition", {

      initController: function () {
        this.model = new sap.ui.model.json.JSONModel();
        this.getView().setModel(this.model);
        this.getOwnerComponent().getRouter().getRoute("StationDefinition").attachPatternMatched(this.onRouteMatched, this);
      },

      // #region Methods
      resetModel: function () {
        this.model.setData({
          station: {
            id: "",
            name: "",
            maxWattPeak: "",

          },
          nameState: "None",
          maxWattPeakState: "None"
        });
      },

      validateInput: function (station) {
        let allValid = true;

        if (this.isNullOrEmpty(station.name)) {
          this.model.setProperty("/nameState", "Error");
          allValid = false;
        }
        else
          this.model.setProperty("/nameState", "None");

        station.maxWattPeak = Number(station.maxWattPeak) || 0;

        if (station.maxWattPeak <= 0) {
          this.model.setProperty("/maxWattPeakState", "Error");
          allValid = false;
        }
        else
          this.model.setProperty("/maxWattPeakState", "None");
          
        return allValid;
      },
      // #endregion

      // #region Events
      onRouteMatched: function (evt) {
        this.resetModel();

        const id = evt.getParameters().arguments.id;
        if (id) {
          const container = this.byId("myPage");
          container.setBusy(true);
          Connector.get("StationDefinition/" + id,
            this.onApiGetStation.bind(this),
            this.handleApiError.bind(this),
            () => container.setBusy(false));
        }
        else
          this.setFocus("nameInput", 100);
      },

      onBackPress: function () {
        this.navigateTo("StationDefinitionOverview");
      },

      onSavePress: function () {
        const station = this.model.getProperty("/station");

        if (!this.validateInput(station))
          return;

        const container = this.byId("myPage");
        container.setBusy(true);
        if (station.id === 0)
          Connector.post("StationDefinition", station,
            this.onApiAddStation.bind(this),
            this.handleApiError.bind(this),
            () => container.setBusy(false));
        else
          Connector.patch("StationDefinition", station,
            this.onApiUpdateStation.bind(this),
            this.handleApiError.bind(this),
            () => container.setBusy(false));
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

        this.model.setProperty("/station", response);
      }
      // #endregion
    });
  });