sap.ui.define([], function () {
  "use strict";

  return {

    get: function (urlPath, onSuccessCallback, onErrorCallback, onCompleteCallback) {
      this._invoke("GET", urlPath, undefined, onSuccessCallback, onErrorCallback, onCompleteCallback);
    },

    post: function (urlPath, payload, onSuccessCallback, onErrorCallback, onCompleteCallback) {
      this._invoke("POST", urlPath, payload, onSuccessCallback, onErrorCallback, onCompleteCallback);
    },

    _invoke: function (method, urlPath, payload, onSuccessCallback, onErrorCallback, onCompleteCallback,) {
      const apiEndpoint = this._getRoolUrl() + "api/" + urlPath;
      this._apiCall(method, apiEndpoint, payload, onSuccessCallback, onErrorCallback, onCompleteCallback);
    },

    _getRoolUrl: function () {
      return window.location.href.endsWith("#") ? window.location.href.substring(0, window.location.href.length - 1) : window.location.href;
    },

    _apiCall: function (method, apiEndpoint, payload, onSuccessCallback, onErrorCallback, onCompleteCallback) {
      // TODO : Switch to Fetch API : https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API/Using_Fetch
      jQuery.ajax({
        url: apiEndpoint,
        type: method,
        crossDomain: true,
        async: true,
        headers: {
          //"Authorization": "Bearer " + bearerToken,
          "content-type": "application/json",
          "cache-control": "no-cache"
        },
        contentType: "application/json",
        dataType: "json",
        data: (payload) ? JSON.stringify(payload) : "",
        success: (data) => onSuccessCallback(data),
        error: (error) => onErrorCallback(error),
        complete: () => {
          if (onCompleteCallback !== undefined)
            onCompleteCallback();
        }
      });
    }
  };
});