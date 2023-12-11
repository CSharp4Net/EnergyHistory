sap.ui.define([], function () {
  "use strict";

  return {

    get: function (urlPath, onSuccessCallback, onErrorCallback, onCompleteCallback) {
      this._apiCall("GET", urlPath, undefined, onSuccessCallback, onErrorCallback, onCompleteCallback);
    },

    post: function (urlPath, payload, onSuccessCallback, onErrorCallback, onCompleteCallback) {
      this._apiCall("POST", urlPath, payload, onSuccessCallback, onErrorCallback, onCompleteCallback);
    },

    patch: function (urlPath, payload, onSuccessCallback, onErrorCallback, onCompleteCallback) {
      this._apiCall("PATCH", urlPath, payload, onSuccessCallback, onErrorCallback, onCompleteCallback);
    },

    delete: function (urlPath, payload, onSuccessCallback, onErrorCallback, onCompleteCallback) {
      this._apiCall("DELETE", urlPath, payload, onSuccessCallback, onErrorCallback, onCompleteCallback);
    },

    //_invoke: function (method, urlPath, payload, onSuccessCallback, onErrorCallback, onCompleteCallback,) {
    //  const apiEndpoint = this._getRoolUrl() + "api/" + urlPath;
    //  this._apiCall(method, apiEndpoint, payload, onSuccessCallback, onErrorCallback, onCompleteCallback);
    //},

    getApiRootUrl: function () {
      const rootUrl = window.location.href.indexOf("#") > -1 ?
        window.location.href.substring(0, window.location.href.indexOf("#")) :
        window.location.href;

      if (!rootUrl.endsWith("/"))
        rootUrl += "/";

      return rootUrl;
    },

    /**
     * Executes asyncron api call and triggers event handler with response or error.
     * @param {string} url URL to which the request is sent
     * @param {string} method Type of request method
     * @param {any} body [Optional] Body data for request.
     * @param {any} onSuccessCallback Method handler for success response (Status code == 200).
     * @param {any} onErrorCallback Method handler for error response (status code != 200).
     * @param {any} onCompleteCallback [Optional] Handler for post processing, after response processed.
     */
    _apiCall: async function (method, urlPath, body, onSuccessCallback, onErrorCallback, onCompleteCallback) {
      const url = this.getApiRootUrl() + "api/" + urlPath,
        headers = new Headers({
          "Content-Type": "application/json"
        });
        
      const response = await fetch(url, {
        method, // *GET, POST, PUT, DELETE, etc.
        mode: "cors", // no-cors, *cors, same-origin
        cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
        headers,
        redirect: "follow", // manual, *follow, error
        referrerPolicy: "no-referrer", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: (body) ? JSON.stringify(body) : null, // body data type must match "Content-Type" header
      });

      if (response.ok && onSuccessCallback)
        onSuccessCallback(await response.json());
      else if (!response.ok && onErrorCallback)
        onErrorCallback(response);

      if (onCompleteCallback)
        onCompleteCallback();
    },

    //_apiCall: function (method, apiEndpoint, payload, onSuccessCallback, onErrorCallback, onCompleteCallback) {
    //  // TODO : Switch to Fetch API : https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API/Using_Fetch
    //  jQuery.ajax({
    //    url: apiEndpoint,
    //    type: method,
    //    crossDomain: true,
    //    async: true,
    //    headers: {
    //      //"Authorization": "Bearer " + bearerToken,
    //      "content-type": "application/json",
    //      "cache-control": "no-cache"
    //    },
    //    contentType: "application/json",
    //    dataType: "json",
    //    data: (payload) ? JSON.stringify(payload) : "",
    //    success: (data) => onSuccessCallback(data),
    //    error: (error) => onErrorCallback(error),
    //    complete: () => {
    //      if (onCompleteCallback !== undefined)
    //        onCompleteCallback();
    //    }
    //  });
    //}
  };
});