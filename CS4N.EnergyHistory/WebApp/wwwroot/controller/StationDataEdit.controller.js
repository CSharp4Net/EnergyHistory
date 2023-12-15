const localStorageEntry_StationDataToEdit = "StationDataToEdit";

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
          pageTitle: "",
          stationId: "",
          selectedYear: 0,
          selectedMonth: 0,
          stationData: null,
          stationDataHeader: {
            manualInput: true
          },
          stationDataEntries: [],
          collectedTotalState: "None"
        });
      },

      formatPowerValue: function (value) {
        return this.format(this.i18n.getText("text_PowerValue"), value.toLocaleString());
      },

      formatDate: function (date, number) {
        const selectedYear = this.model.getProperty("/selectedYear"),
          selectedMonth = this.model.getProperty("/selectedMonth");

        if (selectedYear > 0) {
          if (selectedMonth > 0)
            return new Date(date).toLocaleDateString();
          else
            return new Date(date).toLocaleString("default", { month: "long", year: "numeric" });
        }
        else
          return number;
      },

      setStationDataEntries: function () {
        let selectedYear = this.model.getProperty("/selectedYear");
        const selectedMonth = this.model.getProperty("/selectedMonth"),
          stationData = this.model.getProperty("/stationData");

        if (selectedYear !== "" && selectedYear > 0) {
          selectedYear = Number(selectedYear);

          const yearData = stationData.years.filter(entry => entry.number === selectedYear);
          if (yearData.length === 0) {
            // Jahr wurde noch nicht erfasst, Template abrufen und erneut ausführen
            Connector.get("StationData/template/" + selectedYear,
              (response) => {
                stationData.years.push(response);
                this.setStationDataEntries();
              },
              this.handleApiError.bind(this));
            return;
          }

          if (selectedMonth > 0) {
            const monthData = yearData[0].months.filter(entry => entry.number === selectedMonth);
            // Alle Tage auflisten
            this.model.setProperty("/stationDataHeader", monthData[0]);
            this.model.setProperty("/stationDataEntries", monthData[0].days);
          }
          else {
            // Alle Monate auflisten
            this.model.setProperty("/stationDataHeader", yearData[0]);
            this.model.setProperty("/stationDataEntries", yearData[0].months);
          }
        }
        else {
          // Alle Jahre auflisten
          this.model.setProperty("/stationDataHeader", stationData);
          this.model.setProperty("/stationDataEntries", stationData.years);
        }

        const stationDataHeader = this.model.getProperty("/stationDataHeader");
        if (stationDataHeader.manualInput && stationDataHeader.collectedTotal === 0) {
          this.model.setProperty("/stationDataHeader/collectedTotal", "");
          this.setFocus("collectedTotalInput", 100);
        }
      },
      // #endregion

      // #region Events
      onRouteMatched: function (evt) {
        this.resetModel();

        const cachedData = localStorage.getItem(localStorageEntry_StationDataToEdit);
        if (this.isNullOrEmpty(cachedData)) {
          this.onBackPress();
          return;
        }

        const data = JSON.parse(cachedData);

        this.model.setProperty("/stationId", data.stationId);
        this.model.setProperty("/selectedYear", data.selectedYear);
        this.model.setProperty("/selectedMonth", data.selectedMonth);

        const container = this.byId("myPage");
        container.setBusy(true);
        Connector.get("StationData/" + data.stationId,
          this.onApiGetStationData.bind(this),
          this.handleApiError.bind(this),
          () => container.setBusy(false));
      },

      onBackPress: function () {
        this.navigateTo("StationData", { id: this.model.getProperty("/stationId") });
      },

      onYearChange: function () {
        this.setStationDataEntries();
      },
      // #endregion

      // #region API-Events
      onApiGetStationData: function (response) {
        if (response.errorMessage) {
          this.showResponseError(response);
          return;
        }

        this.model.setProperty("/pageTitle", this.format(this.i18n.getText("title_StationEdit"), response.stationDefinition.name));
        this.model.setProperty("/stationDefinition", response.stationDefinition);
        this.model.setProperty("/stationData", response.stationData);

        this.setStationDataEntries();
      }
      // #endregion
    });
  });