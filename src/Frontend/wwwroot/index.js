import React from 'react';
import { render } from 'react-dom';
import { Provider } from 'react-redux';
import { applyMiddleware, compose, createStore } from 'redux';
import thunkMiddleware from 'redux-thunk';
import * as actions from './actions';
import rootReducer from './reducers';
import App from './components/App';
import createSignalrMiddleware from './createSignalrMiddleware';

fetch("/getWebApiUrl")
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
                        const shipmentHub = connection.shipmentHub = connection.createHubProxy('shipmentHub');
                        shipmentHub.on('Disconnect', () => dispatch({ type: actions.SIGNALR_CONNECTION_STOP }));
                        shipmentHub.on("shipmentCreated",
                            function (id, address) {
                                dispatch({ type: actions.ADD_SHIPMENT_SUCCESS, newShipment: { id, address } });
                            });
                        shipmentHub.on("showErrorMessage",
                            function (errorMessage) {
                                dispatch({ type: actions.ADD_SHIPMENT_FAILURE, errorMessage });
                            });
                        shipmentHub.on("shipmentAddressChanged",
                            function (id, newAddress) {
                                dispatch({ type: actions.UPDATE_SHIPMENT_SUCCESS, id, newAddress });
                            });
                    });

                    const store = createStore(
                        rootReducer,
                        { connection: { hostUrl: webApiUrl }},
                        compose(applyMiddleware(thunkMiddleware, signalrMiddleware),
                            window.__REDUX_DEVTOOLS_EXTENSION__ && window.__REDUX_DEVTOOLS_EXTENSION__()));

                    store.dispatch({ type: actions.SIGNALR_CONNECTION_START });

                    render(
                        <Provider store={store}>
                            <App />
                        </Provider>,
                        document.getElementById('root')
                    );
                });
        }
    });