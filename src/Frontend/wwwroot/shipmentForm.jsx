import React from 'react';

var ShipmentForm = React.createClass({
    getInitialState: function () {
        return { address: '' };
    },
    handleAddressChange: function (e) {
        this.setState({ address: e.target.value });
    },
    handleSubmit: function (e) {
        e.preventDefault();
        var address = this.state.address.trim();
        if (!address) {
            return;
        }

        this.props.onShipmentSubmit({ address: address });
        this.setState({ address: '' });
    },
    render: function () {
        return (
          <form onSubmit={this.handleSubmit}>
              <h3>Add new shipment:</h3>
              <div className="form-group">
                <label>Shipment address</label>
                <input type="text" className="form-control" value={this.state.address} onChange={this.handleAddressChange} />
              </div>
            <input type="submit" value="Create" />
          </form>
      );
    }
});

export default ShipmentForm;