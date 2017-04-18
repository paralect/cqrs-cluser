import React from 'react';
import ReactDOM from "react-dom";
import ShipmentBox from 'shipmentBox.jsx';

const webApiHost = "https://192.168.2.66/web-api/"; // TODO get from configuration or environment variable

const listUrl =  webApiHost + "api/shipments";
const createUrl = webApiHost + "api/shipments";
const updateUrl = webApiHost + "api/shipments";

const signalRConnectionUrl = webApiHost + "signalr/";
const signalRHubsUrl = webApiHost + "signalr/hubs";

$.getScript(signalRHubsUrl, function () {

   let store = {
       data: [],
       error: { message: null }
   };

   let connection = $.hubConnection(signalRConnectionUrl, { useDefaultPath: false });
   let shipmentHubProxy = connection.createHubProxy("shipmentHub");

   shipmentHubProxy.on("shipmentCreated", function(id, address) {
       console.log("shipment is created");
       store.data.push({ id: id, address: address });
       store.error = { message: null };
       store.onDataUpdated();
   });

   shipmentHubProxy.on("shipmentAddressChanged", function(id, newAddress) {
       console.log("shipment is updated");
       store.data.find(s => s.id === id).address = newAddress;
       store.error = { message: null };
       store.onDataUpdated();
   });

   shipmentHubProxy.on("showErrorMessage", function(message) {
       console.log(message);
       store.error = { message: message };
       store.onError();
   });

   connection.logging = true;

   // https://github.com/SignalR/SignalR/issues/3776
   let getUrl = $.signalR.transports._logic.getUrl;
   $.signalR.transports._logic.getUrl = function(connection, transport, reconnecting, poll, ajaxPost) {
       var url = getUrl(connection, transport, reconnecting, poll, ajaxPost);
       return "/web-api" + url;
   };

   connection.start()
       .done(function() {
            let connectionId = connection.id;
            console.log('Now connected, connection ID=' + connectionId);

            fetch(listUrl)
               .then(response => response.json())
               .then(responseJson => {

                   store.data = responseJson;

                   ReactDOM.render(
                       <ShipmentBox store={store}
                                    createUrl={createUrl}
                                    updateUrl={updateUrl}
                                    connectionId={connectionId} />,
                       document.getElementById('shipments')
                   );
               })
               .catch(error => {
                   console.log(error);
               });
       })
       .fail(function() { console.log('Could not Connect!'); });
});