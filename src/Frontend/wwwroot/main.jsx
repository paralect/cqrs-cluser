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

var ShipmentBox = React.createClass({
    getInitialState: function () {
        return { data: [] };
    },
    componentWillMount: function () {
        var xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        }.bind(this);
        xhr.send();
    },
    render: function() {
        return (
          <div>
            <h1>Shipments</h1>
            <ShipmentList data={this.state.data} />
          </div>
        );
    }
});


ReactDOM.render(
  <ShipmentBox url={"http://localhost:8000/api/shipments" } />,
  document.getElementById('shipments')
);

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

        var data = JSON.stringify(address);

        var xhr = new XMLHttpRequest();
        xhr.open('post', this.props.url, true);
        xhr.setRequestHeader('Content-Type', 'application/json; charset=UTF-8');
        xhr.send(data);

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

ReactDOM.render(
  <ShipmentForm url={"http://localhost:8000/api/shipments" } />,
  document.getElementById('shipmentForm')
);