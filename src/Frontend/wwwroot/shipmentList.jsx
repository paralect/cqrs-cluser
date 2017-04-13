import React from 'react';
import ShipmentRow from 'shipmentRow.jsx';

class ShipmentList extends React.Component {

    constructor(props) {
        super(props);
        this.handleShipmentUpdate = this.handleShipmentUpdate.bind(this);
    }

    handleShipmentUpdate (shipment) {
        fetch(this.props.updateUrl + '/' + shipment.id,
            {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Connection-Id': this.props.connectionId
                },
                body: JSON.stringify(shipment.newAddress)
            }).catch(error => {
                console.log(error);
            });
    }

    render () {
        var handleShipmentUpdate = this.handleShipmentUpdate;
        var shipmentRows = this.props.data.map(function (data) {
            return <ShipmentRow onShipmentUpdate={handleShipmentUpdate} key={data.id} data={data} />;
        });
        return (
           <table className="table table-sm">
               <thead>
                   <tr>
                       <th>Id</th>
                       <th>Address</th>
                       <th></th>
                   </tr>
               </thead>
               <tbody>
                   {shipmentRows}
               </tbody>
           </table>
        );
    }
};

export default ShipmentList;