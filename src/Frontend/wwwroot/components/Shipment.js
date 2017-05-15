import React, { PropTypes } from 'react';

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