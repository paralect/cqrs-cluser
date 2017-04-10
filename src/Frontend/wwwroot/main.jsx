var ShipmentList = React.createClass({
    render: function () {
        var shipmentNodes = this.props.data.map(function (data) {
            return (
                <tr key={data.id}>
                    <td>{data.id}</td>
                    <td>{data.address}</td>
                </tr>
              );
        });
        return (
           <table className="table">
               <thead>
                   <tr>
                       <th>Id</th>
                       <th>Address</th>
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
            <ShipmentList data={this.state.data} />
            <ShipmentForm onShipmentSubmit={this.handleShipmentSubmit} />
          </div>
        );
    }
});

ReactDOM.render(
  <ShipmentBox url={"http://localhost:8000/api/shipments"} submitUrl={"http://localhost:8000/api/shipments"} pollInterval={2000}/>,
  document.getElementById('shipments')
);