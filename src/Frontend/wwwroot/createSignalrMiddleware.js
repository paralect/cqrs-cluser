import { fetchShipments } from './actions';

export default hostUrl => actionDispatcher => store => {

    const dispatch = store.dispatch.bind(store);
    const stateConversion = {
        0: 'connecting',
        1: 'connected',
        2: 'reconnecting',
        4: 'disconnected'
    };

    let keepAlive = false;
    let wasConnected = false;
    let currentState = null;

    const connection = $.hubConnection(`${hostUrl}/signalr`);

    actionDispatcher(dispatch, connection);

    function onStateChanged(state) {
        if (currentState === state) {
            return;
        }
        currentState = state;
        dispatch({
            type: 'connection:statechanged',
            state: state
        });
    }

    connection.stateChanged(state => {
        const newStateName = stateConversion[state.newState];
        if (newStateName === 'connected') {
            wasConnected = true;
            onStateChanged('connected');
            dispatch({
                type: 'connection:invoke',
                hub: 'shipmentHub',
                method: 'listen',
                args: connection.id
            });
        }
    });

    // When the connection drops, try to reconnect.
    connection.disconnected(function () {
        if (keepAlive) {
            if (wasConnected) {
                onStateChanged('reconnecting')
            } else {
                onStateChanged('connecting')
            }
            connection.start();
        }
    });

    return next => action => {
        const { type } = action;
        switch (type) {
            case 'connection:start':
                keepAlive = true;
                onStateChanged('connecting');
                connection.start().done(() => {
                    onStateChanged('connected');
                });
                return;
            case 'connection:stop':
                keepAlive = false;
                wasConnected = false;
                onStateChanged('disconnected')
                connection.stop();
                return;
            case 'connection:invoke':
                const { hub, method, args } = action;
                const proxy = connection[hub];
                proxy.invoke(method, args).done(() => {
                    dispatch(fetchShipments(`${hostUrl}/api/shipments`));
                });
                return;
            default:
                return next(action);
        }
    };
};