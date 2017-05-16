import { combineReducers } from 'redux';
import { reducer as formReducer } from 'redux-form';
import * as actions from './actions';

// const shipment = (state = { editMode: false }, action) => {
//     switch (action.type) {
//         case actions.RECEIVE_SHIPMENTS:
//             return Object.assign({}, state, );
//         case actions.ENTER_EDIT_MODE:
//             return Object.assign({}, state, {
//                 editMode: true,
//                 itemToEdit: action.id
//             });
//         case actions.EXIT_EDIT_MODE:
//             return Object.assign({}, state, {
//                 editMode: false,
//                 itemToFinishEdit: action.id
//             });
//         case actions.ADD_SHIPMENT_SUCCESS: 
//             return {
//                 id: action.newShipment.id,
//                 address: action.newShipment.address,
//             };
//         default:
//             return state;
//     }
// }

const shipments = (state = { items: [], isFetching: false }, action) => {
    switch (action.type) {
        case actions.REQUEST_SHIPMENTS:
            return Object.assign({}, state, { isFetching: true });
        case actions.RECEIVE_SHIPMENTS:
            return Object.assign({}, state, {
                isFetching: false,
                items: action.items.map(i => {
                    return {
                        id: i.id,
                        address: i.address,
                        editMode: false
                    };
                })
            });
        case actions.ADD_SHIPMENT_REQUEST:
            return Object.assign({}, state, { isFetching: true });
        case actions.ADD_SHIPMENT_SUCCESS:
            return Object.assign({}, state, {
                isFetching: false,
                items: [...state.items, { 
                    id: action.newShipment.id, 
                    address: action.newShipment.address, 
                    editMode: false 
                }],
                errorMessage: null
            });
        case actions.ADD_SHIPMENT_FAILURE:
            return Object.assign({}, state, {
                isFetching: true,
                errorMessage: action.errorMessage
            });
        case actions.ENTER_EDIT_MODE:
            return Object.assign({}, state,
                { items: state.items.map(i => i.id === action.id ? Object.assign({}, i, { editMode: true }) : i)}
            );
        case actions.EXIT_EDIT_MODE:
            return Object.assign({}, state,
                { items: state.items.map(i => i.id === action.id ? Object.assign({}, i, { editMode: false }) : i)}
            );
        case actions.UPDATE_SHIPMENT_SUCCESS:
            return Object.assign({}, state,
                { items: state.items.map(i => i.id === action.id ? Object.assign({}, i, { address: action.newAddress }) : i)}
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
    form: formReducer
});

export default rootReducer;