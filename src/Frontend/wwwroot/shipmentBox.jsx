import React from 'react';
import ShipmentList from 'shipmentList.jsx';
import ShipmentForm from 'shipmentForm.jsx';

class ShipmentBox extends React.Component {

    constructor(props) {
        super(props);
        this.state = { data: this.props.store.data, error: { message: null } }
        this.handleShipmentCreation = this.handleShipmentCreation.bind(this);
    }

    componentWillMount(){
        this.props.store.onDataUpdated = () => {
            this.setState({ data: this.props.store.data, error: this.props.store.error });
        };

        this.props.store.onError = () => {
            this.setState({ data: this.props.store.data, error: this.props.store.error });
        }
    }

    handleShipmentCreation (shipment) {
        fetch(this.props.createUrl,
             {
                 method: 'POST',
                 headers: {
                     'Content-Type': 'application/json',
                     'Connection-Id': $.connection.hub.id
                 },
                 body: JSON.stringify(shipment.address)
             })
             .then(response => response.json())
             .then(responseJson => {
                 console.log(responseJson);
             })
             .catch(error => {
                 console.log(error);
             });
    }

    render () {
        return (
          <div>
            <h1>Shipments</h1>
            <ShipmentList updateUrl={"http://localhost:5001/api/shipments"}
                          data={this.state.data}/>
            <ShipmentForm onShipmentSubmit={this.handleShipmentCreation} errorMessage={this.state.error.message} />
          </div>
        );
    }
};

export default ShipmentBox;