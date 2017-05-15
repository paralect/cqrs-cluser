import React from 'react';
import { render } from 'react-dom';
import { Provider } from 'react-redux';
import { createStore } from 'redux';
import shipmentApp from './reducers';
import App from './components/App';

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
                function () {

                    let store = {
                        data: [],
                        error: { message: null }
                    };

                    let connection = $.hubConnection(signalRConnectionUrl, { useDefaultPath: false });
                    let shipmentHubProxy = connection.createHubProxy("shipmentHub");

                    shipmentHubProxy.on("shipmentCreated",
                        function (id, address) {
                            console.log("shipment is created");
                            store.data.push({ id: id, address: address });
                            store.error = { message: null };
                            store.onDataUpdated();
                        });

                    shipmentHubProxy.on("shipmentAddressChanged",
                        function (id, newAddress) {
                            console.log("shipment is updated");
                            store.data.find(s => s.id === id).address = newAddress;
                            store.error = { message: null };
                            store.onDataUpdated();
                        });

                    shipmentHubProxy.on("showErrorMessage",
                        function (message) {
                            console.log(message);
                            store.error = { message: message };
                            store.onError();
                        });

                    // https://github.com/SignalR/SignalR/issues/3776
                    /*let getUrl = $.signalR.transports._logic.getUrl;
                    $.signalR.transports._logic.getUrl = function(connection, transport, reconnecting, poll, ajaxPost) {
                        connection.baseUrl = webApiUrl;
                        var url = getUrl(connection, transport, reconnecting, poll, ajaxPost);
                        return transport === "webSockets" ? "/web-api" + url : url;
                    };*/

                    connection.logging = true;
                    connection.start()
                        .done(function () {
                            let connectionId = connection.id;
                            console.log('Now connected, connection ID=' + connectionId);

                            shipmentHubProxy.invoke("listen", connectionId).done(function () {
                                fetch(listUrl)
                                    .then(response => response.json())
                                    .then(responseJson => {

                                        let store = createStore(shipmentApp, { shipments: responseJson });

                                        render(
                                            <Provider store={store}>
                                                <App />
                                            </Provider>,
                                            document.getElementById('root')
                                        );

                                    })
                                    .catch(error => {
                                        console.log(error);
                                    });
                            });
                        })
                        .fail(function () { console.log('Could not Connect!'); });
                });
        }
    });

