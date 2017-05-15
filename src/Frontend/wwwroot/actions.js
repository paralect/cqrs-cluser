export const ADD_SHIPMENT = 'ADD_SHIPMENT';
export const CHANGE_SHIPMENT_ADDRESS = 'CHANGE_SHIPMENT_ADDRESS';

export function addShipment(address) {
    return { type: ADD_SHIPMENT, address }
}

export function changeShipmentAddress(newAddress) {
    return { type: CHANGE_SHIPMENT_ADDRESS, newAddress }
}
