import { combineReducers } from 'redux';
import { ADD_SHIPMENT, CHANGE_SHIPMENT_ADDRESS } from './actions';

function shipments(state = [], action) {
    switch (action.type) {
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