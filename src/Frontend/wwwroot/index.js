import React from 'react';
import { render } from 'react-dom';
import { Provider } from 'react-redux';
import { applyMiddleware, compose, createStore } from 'redux';
import thunkMiddleware from 'redux-thunk';
import { ADD_SHIPMENT_SUCCESS } from './actions';
import shipmentApp from './reducers';
import App from './components/App';
import createSignalrMiddleware from './createSignalrMiddleware';

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

                    let signalrMiddleware = createSignalrMiddleware(webApiUrl);
                    signalrMiddleware = signalrMiddleware((dispatch, connection) => {
                        const shipmentHub = connection['shipmentHub'] = connection.createHubProxy('shipmentHub');
                        shipmentHub.on('Disconnect', () => dispatch({ type: 'connection:stop' }));
                        shipmentHub.on("shipmentCreated",
                            function (id, address) {
                                dispatch({ type: ADD_SHIPMENT_SUCCESS, newShipment: { id, address } });
                            });
                    });

                    // let store = {
                    //     data: [],
                    //     error: { message: null }
                    // };

                    // shipmentHubProxy.on("shipmentAddressChanged",
                    //     function (id, newAddress) {
                    //         console.log("shipment is updated");
                    //         store.data.find(s => s.id === id).address = newAddress;
                    //         store.error = { message: null };
                    //         store.onDataUpdated();
                    //     });

                    // shipmentHubProxy.on("showErrorMessage",
                    //     function (message) {
                    //         console.log(message);
                    //         store.error = { message: message };
                    //         store.onError();
                    //     });

                    // https://github.com/SignalR/SignalR/issues/3776
                    /*let getUrl = $.signalR.transports._logic.getUrl;
                    $.signalR.transports._logic.getUrl = function(connection, transport, reconnecting, poll, ajaxPost) {
                        connection.baseUrl = webApiUrl;
                        var url = getUrl(connection, transport, reconnecting, poll, ajaxPost);
                        return transport === "webSockets" ? "/web-api" + url : url;
                    };*/

                    let store = createStore(
                        shipmentApp,
                        compose(applyMiddleware(thunkMiddleware, signalrMiddleware),
                            window.__REDUX_DEVTOOLS_EXTENSION__ && window.__REDUX_DEVTOOLS_EXTENSION__()));

                    store.dispatch({ type: 'connection:start' });

                    render(
                        <Provider store={store}>
                            <App />
                        </Provider>,
                        document.getElementById('root')
                    );
                });
        }
    });