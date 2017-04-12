import $ from 'jquery';
window.$ = window.jQuery = $;

import React from 'react';
import ReactDOM from "react-dom";
import ShipmentBox from 'shipmentBox.jsx';

ReactDOM.render(
  <ShipmentBox createUrl={"http://localhost:5001/api/shipments"}
               listUrl={"http://localhost:5001/api/shipments"} />,
  document.getElementById('shipments')
);