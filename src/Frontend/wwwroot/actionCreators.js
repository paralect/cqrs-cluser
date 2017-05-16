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

export function changeShipmentAddress(newAddress) {
    return { type: actions.CHANGE_SHIPMENT_ADDRESS, newAddress };
}

export function enterEditMode(id) {
    return { type: actions.ENTER_EDIT_MODE, id }
}

export function exitEditMode(id) {
    return { type: actions.EXIT_EDIT_MODE, id }
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
        dispatch(addShipmentRequest());
        fetch(`${getState().connection.hostUrl}/api/shipments`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Connection-Id': getState().connection.connectionId
            },
            body: JSON.stringify(values.shipmentAddress)
        });
    };
}