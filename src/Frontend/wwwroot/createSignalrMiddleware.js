import { fetchShipments } from './actionCreators';
import * as actions from './actions';
import * as signalr from './signalrStates';

export default actionDispatcher => store => {

    const dispatch = store.dispatch.bind(store);
    const stateConversion = {
        0: signalr.CONNECTING,
        1: signalr.CONNECTED,
        2: signalr.RECONNECTING,
        4: signalr.DISCONNECTED
    };

    let keepAlive = false;
    let wasConnected = false;
    let currentState = null;

    const hostUrl = store.getState().connection.hostUrl;
    const connection = $.hubConnection(hostUrl);

    actionDispatcher(dispatch, connection);

    function onStateChanged(state) {
        if (currentState === state) {
            return;
        }
        currentState = state;
        dispatch({
            type: actions.SIGNALR_CONNECTION_STATE_CHANGED,
            state: state
        });
    }

    connection.stateChanged(state => {
        const newStateName = stateConversion[state.newState];
        if (newStateName === signalr.CONNECTED) {
            wasConnected = true;
            onStateChanged(signalr.CONNECTED);

            dispatch({
                type: actions.SET_CONNECTION_ID,
                connectionId: connection.id
            });

            dispatch({
                type: actions.SIGNALR_INVOKE_METHOD,
                hub: 'shipmentHub',
                method: 'listen',
                args: connection.id
            });

            dispatch(fetchShipments(`${hostUrl}/api/shipments`));
        }
    });

    // When the connection drops, try to reconnect.
    connection.disconnected(function () {
        if (keepAlive) {
            if (wasConnected) {
                onStateChanged(signalr.RECONNECTING);
            } else {
                onStateChanged(signalr.CONNECTING);
            }
            connection.start().done(() => {
                onStateChanged(signalr.CONNECTED);
            });
        }
    });

    return next => action => {
        switch (action.type) {
            case actions.SIGNALR_CONNECTION_START:
                keepAlive = true;
                onStateChanged(signalr.CONNECTING);
                connection.start().done(() => {
                    onStateChanged(signalr.CONNECTED);
                });
                return;
            case actions.SIGNALR_CONNECTION_STOP:
                keepAlive = false;
                wasConnected = false;
                onStateChanged(signalr.DISCONNECTED);
                connection.stop();
                return;
            case actions.SIGNALR_INVOKE_METHOD:
                const { hub, method, args } = action;
                const proxy = connection[hub];
                proxy.invoke(method, args);
                return;
            default:
                return next(action);
        }
    };
};