import { combineReducers } from 'redux';
import { reducer as formReducer } from 'redux-form';
import * as actions from './actions';

function shipments(state = { items: [], isFetching: false }, action) {
    switch (action.type) {
        case actions.REQUEST_SHIPMENTS:
            return Object.assign({}, state, {
                  isFetching: true
                }
            );
        case actions.RECEIVE_SHIPMENTS:
            return Object.assign({}, state, {
                isFetching: false,
                items: action.items
            });
        case actions.ADD_SHIPMENT_REQUEST:
            return Object.assign({}, state, {
                    isFetching: true
                });
        case actions.ADD_SHIPMENT_SUCCESS:
            return Object.assign({}, state, {
                    isFetching: false,
                    items: [...state.items, action.newShipment]
                });
        case actions.ADD_SHIPMENT_FAILURE:
            return Object.assign({}, state, {
                    isFetching: true,
                    errorMessage: action.errorMessage
                });
        case actions.CHANGE_SHIPMENT_ADDRESS:
            return [
                ...state,
                {
                    address: action.newAddress
                }
            ];
        default:
            return state;
    }
}

function connection(state = { }, action) {
    switch (action.type) {
        case actions.SET_CONNECTION_ID:
            return Object.assign({}, state, {
                connectionId: action.connectionId
            });
        default:
            return state;
    }
}

const rootReducer = combineReducers({
    shipments,
    connection,
    form: formReducer
});

export default rootReducer;