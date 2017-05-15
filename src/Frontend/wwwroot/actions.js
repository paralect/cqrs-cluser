import fetch from 'isomorphic-fetch';

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

// export const FETCH_SHIPMENTS_FAILURE = 'FETCH_SHIPMENTS_FAILURE';
// export function fetchShipmentsFailure(errorMessage) {
//     return { type: FETCH_SHIPMENTS_FAILURE, errorMessage };
// }

export const ADD_SHIPMENT = 'ADD_SHIPMENT';
export function addShipment(address) {
    return { type: ADD_SHIPMENT, address };
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
    }
}