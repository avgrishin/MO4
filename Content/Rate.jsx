var Rate = React.createClass({
  render: function() {
    return (
      <tr>
        <td>{this.props.data.sec}</td>
        <td className="ta-r">{this.props.data.cl}</td>
        <td className="ta-r" style={{color: this.props.data.chg < 0 ? 'red' : 'green'}}>{this.props.data.chg.toFixed(2)}</td>
        <td>{this.props.data.tm}</td>
      </tr>
    );
}
});

var RateList = React.createClass({
  render: function() {
    return (
      <table className="fp-currency" style={{width: 300}}><tbody>
      {
        this.props.data.map(function(result) {
          return (
            <Rate key={result.sec} data={result} />
          );
        })
      }
      </tbody></table>
    );
  }
});

var RateBox = React.createClass({
  getInitialState: function() {
    return {data: this.props.initialData};
  },
  loadRatesFromServer: function() {
    $.ajax({
      url: this.props.url,
      dataType: 'json',
      cache: false,
      success: function(data) {
        this.setState({data: data});
      }.bind(this),
      error: function(xhr, status, err) {
        console.error(this.props.url, status, err.toString());
      }.bind(this)
    });
  },
  componentDidMount: function() {
    setInterval(this.loadRatesFromServer, this.props.pollInterval);
  },
  render: function() {
    return (
      <div>
        <h2>Курсы MOEX</h2>
        <RateList data={this.state.data} />
      </div>
    );
  }
});
