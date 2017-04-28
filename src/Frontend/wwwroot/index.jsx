import React from 'react';
import ReactDOM from "react-dom";
import ShipmentBox from 'shipmentBox.jsx';

fetch("/getIp")
    .then(response => response.json())
    .then(responseJson => {

        const webApiUrl = responseJson.webApiUrl;
        if (!webApiUrl) {
            console.log("WebApi is not available, please try again later");
        } else {
            const listUrl = webApiUrl + "/api/shipments";
            const createUrl = webApiUrl + "/api/shipments";
            const updateUrl = webApiUrl + "/api/shipments";

            const signalRConnectionUrl = webApiUrl + "/signalr";
            const signalRHubsUrl = webApiUrl + "/signalr/hubs";

            $.getScript(signalRHubsUrl,
                function() {

                    let store = {
                        data: [],
                        error: { message: null }
                    };

                    let connection = $.hubConnection(signalRConnectionUrl, { useDefaultPath: false });
                    let shipmentHubProxy = connection.createHubProxy("shipmentHub");

                    shipmentHubProxy.on("shipmentCreated",
                        function(id, address) {
                            console.log("shipment is created");
                            store.data.push({ id: id, address: address });
                            store.error = { message: null };
                            store.onDataUpdated();
                        });

                    shipmentHubProxy.on("shipmentAddressChanged",
                        function(id, newAddress) {
                            console.log("shipment is updated");
                            store.data.find(s => s.id === id).address = newAddress;
                            store.error = { message: null };
                            store.onDataUpdated();
                        });

                    shipmentHubProxy.on("showErrorMessage",
                        function(message) {
                            console.log(message);
                            store.error = { message: message };
                            store.onError();
                        });

                    connection.logging = true;
                    connection.start()
                        .done(function() {
                            let connectionId = connection.id;
                            console.log('Now connected, connection ID=' + connectionId);

                            shipmentHubProxy.invoke("listen", connectionId).done(function() {
                                fetch(listUrl)
                                    .then(response => response.json())
                                    .then(responseJson => {
                                        store.data = responseJson;
                                        ReactDOM.render(
                                            <ShipmentBox store={store}
                                                         createUrl={createUrl}
                                                         updateUrl={updateUrl}
                                                         connectionId={connectionId}/>,
                                            document.getElementById('shipments')
                                        );
                                    })
                                    .catch(error => {
                                        console.log(error);
                                    });
                            });
                        })
                        .fail(function() { console.log('Could not Connect!'); });
                });
        }
    });