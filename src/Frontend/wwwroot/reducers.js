import { combineReducers } from 'redux';
import { REQUEST_SHIPMENTS, RECEIVE_SHIPMENTS, ADD_SHIPMENT, CHANGE_SHIPMENT_ADDRESS } from './actions';

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
        case ADD_SHIPMENT:
            return [
                ...state,
                {
                    address: action.address
                }
            ];
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