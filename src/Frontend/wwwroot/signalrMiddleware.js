const connection = $.hubConnection('/signalr', {
    useDefaultPath: false
});

export default function createSignalrMiddleware(actionDispatcher) {
    return store => {

        const dispatch = store.dispatch.bind(store)
        const stateConversion = {
            0: 'connecting',
            1: 'connected',
            2: 'reconnecting',
            4: 'disconnected'
        };

        let keepAlive = false;
        let wasConnected = false;
        let currentState = null;

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
                    keepAlive = true
                    onStateChanged('connecting')
                    connection.start({
                        transport: [
                            /* 'webSockets', */
                            'longPolling'
                        ]
                    })
                    return;
                case 'connection:stop':
                    keepAlive = false;
                    wasConnected = false;
                    onStateChanged('disconnected');
                    connection.stop();
                    return;
                case 'connection:invoke':
                    const { hub, method, args } = action;
                    const proxy = connection[hub];
                    proxy.invoke(method, ...args);
                    return;
                default:
                    return next(action);
            }
        };
    };
}