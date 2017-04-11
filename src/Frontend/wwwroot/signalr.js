var shipmentHub = $.connection.shipmentHub;

shipmentHub.client.shipmentCreated = function () {
    console.log('shipment is created');
}

$.connection.hub.logging = true;
$.connection.hub.start()
   .done(function () { console.log('Now connected, connection ID=' + $.connection.hub.id); })
   .fail(function () { console.log('Could not Connect!'); });