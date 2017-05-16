import React from 'react';
import AddShipmentForm from '../containers/AddShipmentForm';
import ShipmentList from '../containers/ShipmentList';

const App = () => (
    <div>
      <h1>Shipments</h1>
      <ShipmentList />
      <AddShipmentForm />
    </div>
);

export default App;