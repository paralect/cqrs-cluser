import React from 'react';
import ShipmentNode from 'shipmentNode.jsx';

var ShipmentRow = React.createClass({
    getInitialState: function () {
        return {
            editMode: false
        };
    },
    render: function () {
        return (
                <tr>
                    <td>{this.props.data.id}</td>
                     <ShipmentNode id={this.props.data.id} address={this.props.data.address} editMode={this.state.editMode} onShipmentUpdate={this.onShipmentUpdate} />
                    <td><button onClick={this.toggleEditMode} className="btn btn-link">Edit</button></td>
                </tr>
              );
    },
    toggleEditMode: function () {
        this.setState({ editMode: !this.state.editMode });
    },
    onShipmentUpdate: function (shipment) {
        this.props.onShipmentUpdate(shipment);
        this.setState({ editMode: false });
    }
});

export default ShipmentRow;