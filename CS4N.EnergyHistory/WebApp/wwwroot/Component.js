sap.ui.define([
    "sap/ui/core/UIComponent",
    "sap/ui/core/ComponentSupport"],
    (UIComponent) => {
        "use strict";
        return UIComponent.extend("CS4N.EnergyHistory.Component", {
            metadata: {
                manifest: "json"
            },

            init: function () {
                UIComponent.prototype.init.apply(this, arguments);

                const i18nModel = new sap.ui.model.resource.ResourceModel({
                    bundleName: "CS4N.EnergyHistory.i18n.i18n"
                });

                sap.ui.getCore().setModel(i18nModel, "i18n");

                this.getRouter().initialize();
            }
        });
    }
);