var ValueList = React.createClass({
    render: function () {
        var valueNodes = this.props.data.map(function (value, index) {
            return (
                  <h2 key={index}>{value}</h2>
              );
        });
        return (
           <div className="valueList">
               {valueNodes}
           </div>
        );
    }
});

var ValueBox = React.createClass({
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
          <div className="valueBox">
            <h1>Values</h1>
            <ValueList data={this.state.data} />
          </div>
        );
    }
});

ReactDOM.render(
  <ValueBox url={"http://localhost:8000/api/values" } />,
  document.getElementById('content')
);