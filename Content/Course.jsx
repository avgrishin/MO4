var Course = React.createClass({
  render: function() {
    return (
      <tr>
        <td>{this.props.data.dt.substr(0, 10).replace(/(\d+)-(\d+)-(\d+)/, '$3.$2.$1')}</td>
        <td>{this.props.data.s}</td>
        <td className="ta-r">{this.props.data.c.toFixed(4)}</td>
        <td className="ta-r" style={{color: this.props.data.y < 0 ? 'red' : 'green'}}>{this.props.data.y.toFixed(4)}</td>
      </tr>
    );
}
});

var CourseList = React.createClass({
  render: function() {
    return (
      <table className="fp-currency" style={{width: 300}}><tbody>
        {this.props.data.map(function(result) {
          return (
            <Course key={result.s} data={result} />
          );
        })}
      </tbody></table>
    );
  }
});

var CourseBox = React.createClass({
  getInitialState: function() {
    return {data: this.props.initialData};
  },
  loadCoursesFromServer: function() {
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
    setInterval(this.loadCoursesFromServer, this.props.pollInterval);
  },
  render: function() {
    return (
      <div>
        <h2>Курсы валют ЦБ РФ</h2>
        <CourseList data={this.state.data} />
      </div>
    );
  }
});
