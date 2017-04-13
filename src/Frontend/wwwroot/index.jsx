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

        var connection = $.hubConnection();
        var shipmentHubProxy = connection.createHubProxy("shipmentHub");
        shipmentHubProxy.on("shipmentCreated", function(id, address) {
            console.log("shipment is created");
            store.data.push({ id: id, address: address });
            store.error = { message: null };
            store.onDataUpdated();
        });

        shipmentHubProxy.on("shipmentAddressChanged", function(id, newAddress) {
            console.log("shipment is updated");
            store.data.find(s => s.id === id).address = newAddress;
            store.error = { message: null };
            store.onDataUpdated();
        });

        shipmentHubProxy.on("showErrorMessage", function(message) {
            console.log(message);
            store.error = { message: message };
            store.onError();
        });

        connection.logging = true;
        connection.url = "http://localhost:5001/signalr/hubs";
        connection.start()
            .done(function() { console.log('Now connected, connection ID=' + connection.id); })
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