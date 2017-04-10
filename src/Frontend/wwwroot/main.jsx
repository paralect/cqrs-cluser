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

var ShipmentRow = React.createClass({
    getInitialState: function () {
        return {
            editMode: false
        };
    },
    render: function() {
        return (
                <tr>
                    <td>{this.props.data.id}</td>
                     <ShipmentNode id={this.props.data.id} address={this.props.data.address} editMode={this.state.editMode} onShipmentUpdate={this.onShipmentUpdate}/>
                    <td><button onClick={this.toggleEditMode} className="btn btn-link">Edit</button></td>
                </tr>
              );
    },
    toggleEditMode: function() {
        this.setState({ editMode: !this.state.editMode });
    },
    onShipmentUpdate: function (shipment) {
        this.props.onShipmentUpdate(shipment);
        this.setState({ editMode: false });
    }
});

var ShipmentList = React.createClass({
    handleShipmentUpdate: function (shipment) {
        fetch(this.props.updateUrl + '/' + shipment.id,
            {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(shipment.newAddress)
            }).catch(error => {
                console.log(error);
            });
    },
    render: function () {
        var handleShipmentUpdate = this.handleShipmentUpdate;
        var shipmentNodes = this.props.data.map(function (data) {
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
                   {shipmentNodes}
               </tbody>
           </table>
        );
    }
});

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

        this.props.onShipmentSubmit({address: address });
        this.setState({ address: '' });
    },
    render: function() {
        return (
          <form onSubmit={this.handleSubmit}>
              <h3>Add new shipment:</h3>
              <div className="form-group">
                <label>Shipment address</label>
                <input type="text" className="form-control" value={this.state.address} onChange={this.handleAddressChange}/>
              </div>
            <input type="submit" value="Create" />
          </form>
      );
    }
});

var ShipmentBox = React.createClass({
    loadShipments: function () {
        fetch(this.props.url)
            .then(response => response.json())
            .then(responseJson => {
                this.setState({ data: responseJson });
            })
            .catch(error => {
                console.log(error);
            });
    },
    handleShipmentSubmit: function (shipment) {
        fetch(this.props.submitUrl,
            {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(shipment.address)
            }).catch(error => {
                console.log(error);
            });
    },
    getInitialState: function () {
        return { data: [] };
    },
    componentDidMount: function() {
        this.loadShipments();
        window.setInterval(this.loadShipments, this.props.pollInterval);
    },
    render: function() {
        return (
          <div>
            <h1>Shipments</h1>
            <ShipmentList data={this.state.data} updateUrl={"http://localhost:8000/api/shipments"} />
            <ShipmentForm onShipmentSubmit={this.handleShipmentSubmit} />
          </div>
        );
    }
});

ReactDOM.render(
  <ShipmentBox url={"http://localhost:8000/api/shipments"} 
               submitUrl={"http://localhost:8000/api/shipments"}
               pollInterval={2000}/>,
  document.getElementById('shipments')
);