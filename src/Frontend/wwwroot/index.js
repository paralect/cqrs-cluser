import React from 'react';
import { render } from 'react-dom';
import { Provider } from 'react-redux';
import { applyMiddleware, compose, createStore } from 'redux';
import thunkMiddleware from 'redux-thunk';
import * as actions from './actions';
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
            const signalRHubsUrl = `${webApiUrl}/signalr/hubs`;
            $.getScript(signalRHubsUrl,
                function () {
                    const signalrMiddleware = createSignalrMiddleware((dispatch, connection) => {
                        const shipmentHub = connection['shipmentHub'] = connection.createHubProxy('shipmentHub');
                        shipmentHub.on('Disconnect', () => dispatch({ type: 'connection:stop' }));
                        shipmentHub.on("shipmentCreated",
                            function (id, address) {
                                dispatch({ type: actions.ADD_SHIPMENT_SUCCESS, newShipment: { id, address } });
                            });
                        shipmentHub.on("showErrorMessage",
                            function (errorMessage) {
                                dispatch({ type: actions.ADD_SHIPMENT_FAILURE, errorMessage });
                            });
                    });

                    // shipmentHubProxy.on("shipmentAddressChanged",
                    //     function (id, newAddress) {
                    //         console.log("shipment is updated");
                    //         store.data.find(s => s.id === id).address = newAddress;
                    //         store.error = { message: null };
                    //         store.onDataUpdated();
                    //     });

                    let store = createStore(
                        shipmentApp,
                        { connection: { hostUrl: webApiUrl }},
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