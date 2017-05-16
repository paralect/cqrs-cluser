import React from 'react';
import PropTypes from 'prop-types';
import Shipment from './Shipment';

const ShipmentList = ({ shipments }) => (
    <table className="table table-sm">
        <thead>
            <tr>
                <th>Id</th>
                <th>Address</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            {shipments.map(shipment =>
                <Shipment key={shipment.id}
                    {...shipment}
                />
            )}
        </tbody>
    </table>
);

ShipmentList.propTypes = {
    shipments: PropTypes.arrayOf(PropTypes.shape({
        id: PropTypes.string.isRequired,
        address: PropTypes.string.isRequired
    }).isRequired).isRequired
};

export default ShipmentList;