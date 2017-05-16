import React from 'react';
import ShipmentNode from 'shipmentNode.jsx';

class ShipmentRow extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            editMode: false
        };
        this.toggleEditMode = this.toggleEditMode.bind(this);
        this.onShipmentUpdate = this.onShipmentUpdate.bind(this);
    }

    render () {
        return (
                <tr>
                    <td>{this.props.data.id}</td>
                     <ShipmentNode id={this.props.data.id} address={this.props.data.address} editMode={this.state.editMode} onShipmentUpdate={this.onShipmentUpdate} />
                    <td><button onClick={this.toggleEditMode} className="btn btn-link">Edit</button></td>
                </tr>
              );
    }

    toggleEditMode () {
        this.setState({ editMode: !this.state.editMode });
    }

    onShipmentUpdate (shipment) {
        this.props.onShipmentUpdate(shipment);
        this.setState({ editMode: false });
    }
};

export default ShipmentRow;