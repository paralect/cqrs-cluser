import React from 'react';
import PropTypes from 'prop-types';
import EditShipmentForm from '../containers/EditShipmentForm';

const Shipment = ({ id, address, editMode, onEdit }) => (
    <tr>
        <td>{id}</td>
        <td>{editMode ? (
            <EditShipmentForm id={id} address={address} form={`EditShipmentForm_${id}`}/>
        ) : address }</td>
        <td><button className="btn btn-link" onClick={() => onEdit(id)}>Edit</button></td>
    </tr>
);
   
Shipment.propTypes = {
    id: PropTypes.string.isRequired,
    address: PropTypes.string.isRequired
}

export default Shipment;