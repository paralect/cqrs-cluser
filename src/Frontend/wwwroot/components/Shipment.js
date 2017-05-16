import React from 'react';
import PropTypes from 'prop-types';

const Shipment = ({ id, address, editMode, onEdit }) => (
    <tr>
        <td>{id}</td>
        <td>{editMode ? (
            <form className="form-inline">
                <input type="text" className="form-control" value={address} />
                <input type="submit" value="Update" className="form-control" />
            </form>
        ) : address }</td>
        <td><button className="btn btn-link" onClick={() => onEdit(id)}>Edit</button></td>
    </tr>
);
   
Shipment.propTypes = {
    id: PropTypes.string.isRequired,
    address: PropTypes.string.isRequired
}

export default Shipment;