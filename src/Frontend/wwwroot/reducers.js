import { combineReducers } from 'redux';
import { reducer as formReducer } from 'redux-form';
import * as actions from './actions';

function shipments(state = { isFetching: false, items: [] }, action) {
    switch (action.type) {
        case actions.REQUEST_SHIPMENTS:
            return Object.assign({}, state, {
                  isFetching: true, 
                  items: action.items 
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
        case actions.SET_CONNECTION_ID:
            return Object.assign({}, state, {
                    connectionId: action.connectionId
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

const shipmentApp = combineReducers({
    shipments,
    form: formReducer
});

export default shipmentApp;