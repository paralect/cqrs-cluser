import fetch from 'isomorphic-fetch';
import * as actions from './actions';

export function requestShipments() {
    return { type: actions.REQUEST_SHIPMENTS, items: [] };
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
        fetch("http://localhost:5001/api/shipments", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Connection-Id': getState().shipments.connectionId
            },
            body: JSON.stringify(values.shipmentAddress)
        });
    };
}