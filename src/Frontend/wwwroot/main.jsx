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
           <table className="shipmentList">
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
          <div className="shipmentBox">
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