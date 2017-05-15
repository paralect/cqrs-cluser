import { combineReducers } from 'redux';
import { 
    SET_CONNECTION_ID,
    REQUEST_SHIPMENTS,
    RECEIVE_SHIPMENTS,
    ADD_SHIPMENT_REQUEST,
    ADD_SHIPMENT_SUCCESS,
    ADD_SHIPMENT_FAILURE,
    CHANGE_SHIPMENT_ADDRESS } from './actions';

function shipments(state = { isFetching: false, items: [] }, action) {
    switch (action.type) {
        case REQUEST_SHIPMENTS:
            return Object.assign({}, state, {
                  isFetching: true, 
                  items: action.items 
                }
            );
        case RECEIVE_SHIPMENTS:
            return Object.assign({}, state, {
                isFetching: false,
                items: action.items
            });
        case ADD_SHIPMENT_REQUEST:
            return Object.assign({}, state, {
                    isFetching: true,
                    address: action.address
                });
        case ADD_SHIPMENT_SUCCESS:
            return Object.assign({}, state, {
                    isFetching: false,
                    items: [...state.items, action.newShipment]
                });
        case ADD_SHIPMENT_FAILURE:
            return Object.assign({}, state, {
                    isFetching: true,
                    errorMessage: action.errorMessage
                });
        case SET_CONNECTION_ID:
            return Object.assign({}, state, {
                    connectionId: action.connectionId
                });
        case CHANGE_SHIPMENT_ADDRESS:
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
    shipments
});

export default shipmentApp;