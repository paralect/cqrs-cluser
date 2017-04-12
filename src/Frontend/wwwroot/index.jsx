import React from 'react';
import ReactDOM from "react-dom";
import ShipmentBox from 'shipmentBox.jsx';

const listUrl = "http://localhost:5001/api/shipments";
const createUrl = "http://localhost:5001/api/shipments";

fetch(listUrl)
    .then(response => response.json())
    .then(responseJson => {
        
        var store = {
            data : responseJson
        }

        var shipmentHub = $.connection.shipmentHub;
        shipmentHub.client.shipmentCreated = function(id, address) {
            console.log("shipment is created");
            store.data.push({ id: id, address: address });
            store.onDataUpdated();
        };

        $.connection.hub.logging = true;
        $.connection.hub.start()
            .done(function() { console.log('Now connected, connection ID=' + $.connection.hub.id); })
            .fail(function() { console.log('Could not Connect!'); });

        ReactDOM.render(
            <ShipmentBox store={store}
                         createUrl={createUrl} />,
            document.getElementById('shipments')
        );

    })
    .catch(error => {
        console.log(error);
    });