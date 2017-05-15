import React from 'react';
import AddShipment from '../containers/AddShipment';
import ShipmentList from '../containers/ShipmentList';

const App = () => (
    <div>
      <h1>Shipments</h1>
      <ShipmentList />
      <AddShipment />
    </div>
);

export default App;