import fetch from 'isomorphic-fetch';
import * as actions from './actions';

export function requestShipments() {
    return { type: actions.REQUEST_SHIPMENTS };
}

export function receiveShipments(items) {
    return { type: actions.RECEIVE_SHIPMENTS, items };
}

export function addShipmentRequest() {
    return { type: actions.ADD_SHIPMENT_REQUEST };
}

export function addShipmentSuccess(newShipment) {
    return { type: actions.ADD_SHIPMENT_SUCCESS, newShipment };
}

export function addShipmentFailure(errorMessage) {
    return { type: actions.ADD_SHIPMENT_FAILURE, errorMessage };
}

export function enterEditMode(id) {
    return { type: actions.ENTER_EDIT_MODE, id }
}

export function exitEditMode(id) {
    return { type: actions.EXIT_EDIT_MODE, id }
}

export function updateShipmentRequest(id, address) {
    return { type: actions.UPDATE_SHIPMENT_REQUEST, id, address };
}

export function updateShipmentSuccess(id, newAddress) {
    return { type: actions.UPDATE_SHIPMENT_SUCCESS, id, newAddress };
}

export function fetchShipments(url) {
    return dispatch => {
        dispatch(requestShipments());
        return fetch(url)
            .then(response => response.json())
            .then(responseJson => {
                dispatch(receiveShipments(responseJson));
            })
            .catch(error => {
                console.log(error);
            });
    };
}

export function addShipment(values) {
    return (dispatch, getState) => {
        const state = getState();
        dispatch(addShipmentRequest());
        fetch(`${state.connection.hostUrl}/api/shipments`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Connection-Id': state.connection.connectionId
            },
            body: JSON.stringify(values.shipmentAddress)
        });
    };
}

export function updateShipment(values) {
    return (dispatch, getState) => {
        const state = getState();
        dispatch(updateShipmentRequest());
        fetch(`${state.connection.hostUrl}/api/shipments/${values.id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Connection-Id': state.connection.connectionId
            },
            body: JSON.stringify(values.address)
        });
        dispatch({type: actions.EXIT_EDIT_MODE, id: values.id });
    };
}