import React from 'react';
import PropTypes from 'prop-types';

const Shipment = ({ id, address }) => (
    <tr>
        <td>{id}</td>
        <td>{address}</td>
        <td><button className="btn btn-link">Edit</button></td>
    </tr>
);
   
Shipment.propTypes = {
    id: PropTypes.string.isRequired,
    address: PropTypes.string.isRequired
}

export default Shipment;