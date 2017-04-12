import React from 'react';

class ShipmentNode extends React.Component {

    constructor(props) {
        super(props);
        this.state = { 
            newAddress: this.props.address 
        };
        this.handleAddressChange = this.handleAddressChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    render () {
        if (this.props.editMode) {
            return (
                <td>
                    <form onSubmit={this.handleSubmit} className="form-inline">
                         <input type="text" className="form-control" value={this.state.newAddress} onChange={this.handleAddressChange} />
                         <input type="submit" value="Update" className="form-control" />
                    </form>
                </td>
            );
        } else {
            return (
                <td>
                    {this.props.address}
                </td>
            );
        }
    }

    handleAddressChange (e) {
        this.setState({ newAddress: e.target.value });
    }

    handleSubmit (e) {
        e.preventDefault();
        var newAddress = this.state.newAddress.trim();
        if (!newAddress) {
            return;
        }

        this.props.onShipmentUpdate({ id: this.props.id, newAddress: newAddress });
    }
};

export default ShipmentNode;