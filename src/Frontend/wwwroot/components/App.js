import React from 'react';
import AddShipmentForm from '../containers/AddShipmentForm';
import ShipmentList from '../containers/ShipmentList';

export default () => (
    <div>
      <h1>Shipments</h1>
      <ShipmentList />
      <AddShipmentForm />
    </div>
);