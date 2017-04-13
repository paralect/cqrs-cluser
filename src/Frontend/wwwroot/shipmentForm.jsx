import React from 'react';

class ShipmentForm extends React.Component {

    constructor(props) {
        super(props);
        this.state = { address: '' };
        this.handleAddressChange = this.handleAddressChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleAddressChange (e) {
        this.setState({ address: e.target.value });
    }

    handleSubmit (e) {
        e.preventDefault();
        var address = this.state.address.trim();
        if (!address) {
            return;
        }

        this.props.onShipmentSubmit({ address: address });
        this.setState({ address: '' });
    }

    render () {
        // todo refactor
        if (this.props.errorMessage === null) {
            return (
                <form onSubmit={this.handleSubmit}>
                    <h3>Add new shipment:</h3>
                    <div className="form-group row">
                        <label>Shipment address</label>
                        <input type="text" className="form-control" value={this.state.address} onChange={this.handleAddressChange} />
                    </div>
                    <input type="submit" value="Create"/>
                </form>
            );
        } else {
            return (
                <form onSubmit={this.handleSubmit}>
                    <h3>Add new shipment:</h3>
                    <div className="form-group row has-danger">
                        <label>Shipment address</label>
                        <input type="text" className="form-control" value={this.state.address} onChange={this.handleAddressChange} />
                        <div className="form-control-feedback">{this.props.errorMessage}</div>
                    </div>
                    <input type="submit" value="Create"/>
                </form>
            );
        }
    }
};

export default ShipmentForm;