import { combineReducers } from 'redux';
import { reducer as formReducer } from 'redux-form';
import * as actions from './actions';

const shipment = (state = { }, action) => {
    switch (action.type) {
        case actions.RECEIVE_SHIPMENTS:
            return Object.assign({}, state, { editMode: false });
        case actions.ENTER_EDIT_MODE:
            return Object.assign({}, state, { editMode: true });
        case actions.EXIT_EDIT_MODE:
            return Object.assign({}, state, { editMode: false });
        case actions.ADD_SHIPMENT_SUCCESS: 
            return {
                id: action.newShipment.id,
                address: action.newShipment.address,
                editMode: false
            };
        case actions.UPDATE_SHIPMENT_SUCCESS:
            return Object.assign({}, state, { address: action.newAddress });
        default:
            return state;
    }
}

const shipments = (state = { items: [] }, action) => {

    const getShipmentsList = (state, action) => 
        state.items.map(i => i.id === action.id ? shipment(i, action) : i);
    
    switch (action.type) {
        case actions.REQUEST_SHIPMENTS:
            return state;
        case actions.RECEIVE_SHIPMENTS:
            return Object.assign({}, state, {
                items: action.items.map(i => shipment(i, action))
            });
        case actions.ADD_SHIPMENT_REQUEST:
            return Object.assign({}, state);
        case actions.ADD_SHIPMENT_SUCCESS:
            return Object.assign({}, state, {
                items: [...state.items, shipment(null, action)],
                errorMessage: null
            });
        case actions.ADD_SHIPMENT_FAILURE:
            return Object.assign({}, state, {
                errorMessage: action.errorMessage
            });
        case actions.ENTER_EDIT_MODE:
            return Object.assign({}, state,
                { items: getShipmentsList(state, action) }
            );
        case actions.EXIT_EDIT_MODE:
            return Object.assign({}, state,
                { items: getShipmentsList(state, action) }
            );
        case actions.UPDATE_SHIPMENT_SUCCESS:
            return Object.assign({}, state,
                { items: getShipmentsList(state, action) }
            );
        default:
            return state;
    }
}

const connection = (state = {}, action) => {
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
    form: formReducer.plugin({
        addShipmentForm: (state, action) => {
            switch (action.type) {
                case actions.ADD_SHIPMENT_SUCCESS:
                    return undefined;
                default:
                    return state;
            }
        }
    })
});

export default rootReducer;