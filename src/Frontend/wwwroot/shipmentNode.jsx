import React from 'react';
var ShipmentNode = React.createClass({
    getInitialState: function () {
        return {
            address: this.props.address,
            newAddress: this.props.address
        };
    },
    render: function () {
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
                    {this.state.address}
                </td>
            );
        }
    },
    handleAddressChange: function (e) {
        this.setState({ newAddress: e.target.value });
    },
    handleSubmit: function (e) {
        e.preventDefault();
        var newAddress = this.state.newAddress.trim();
        if (!newAddress) {
            return;
        }

        this.props.onShipmentUpdate({ id: this.props.id, newAddress: newAddress });
        this.setState({ address: newAddress });
    }
});

export default ShipmentNode;