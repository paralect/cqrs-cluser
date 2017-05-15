import fetch from 'isomorphic-fetch';

export const SET_CONNECTION_ID = 'SET_CONNECTION_ID';

export const REQUEST_SHIPMENTS = 'REQUEST_SHIPMENTS';
export function requestShipments() {
    return { type: REQUEST_SHIPMENTS, items: [] };
}

export const RECEIVE_SHIPMENTS = 'RECEIVE_SHIPMENTS';
export function receiveShipments(json) {
    return {
        type: RECEIVE_SHIPMENTS,
        items: json
    };
}

export const ADD_SHIPMENT_REQUEST = 'ADD_SHIPMENT_REQUEST';
export function addShipmentRequest(address) {
    return { type: ADD_SHIPMENT_REQUEST, address };
}

export const ADD_SHIPMENT_SUCCESS = 'ADD_SHIPMENT_SUCCESS';
export function addShipmentSuccess(newShipment) {
    return { type: ADD_SHIPMENT_SUCCESS, newShipment };
}

export const ADD_SHIPMENT_FAILURE = 'ADD_SHIPMENT_FAILURE';
export function addShipmentFailure(errorMessage) {
    return { type: ADD_SHIPMENT_FAILURE, errorMessage };
}

export const CHANGE_SHIPMENT_ADDRESS = 'CHANGE_SHIPMENT_ADDRESS';
export function changeShipmentAddress(newAddress) {
    return { type: CHANGE_SHIPMENT_ADDRESS, newAddress };
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

export function addShipment(address) {
    return (dispatch, getState) => {
        dispatch(addShipmentRequest());
        fetch("http://localhost:5001/api/shipments", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Connection-Id': getState().shipments.connectionId
            },
            body: JSON.stringify(address)
        });
    };
}