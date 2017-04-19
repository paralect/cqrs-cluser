namespace ReadModel.EventHandlers
{
    using Contracts.Events;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using ParalectEventSourcing.Events;
    using ParalectEventSourcing.Messaging.RabbitMq;
    using ParalectEventSourcing.Persistence.MongoDb;

    public class ShipmentEventsHandler : IEventHandler
    {
        private const string ShipmentCollection = "shipments";

        private readonly IDatabase _database;
        private readonly ISuccessChannel _channel;

        public ShipmentEventsHandler(IDatabase database, ISuccessChannel channel)
        {
            _database = database;
            _channel = channel;
        }

        public void Handle(DeviceAddedToShipment e)
        {
            
        }

        public void Handle(ShipmentCreated e)
        {
            var collection = _database.GetCollection(ShipmentCollection);
            var shipment = new BsonDocument
            {
                { "id", e.Id },
                { "address", e.Address }
            };

            collection.InsertOne(shipment);

            _channel.Send(ExchangeConfiguration.SuccessExchange, e, e.Metadata.ConnectionToken);
        }

        public void Handle(ShipmentAddressChanged e)
        {
            var collection = _database.GetCollection(ShipmentCollection);
            var filter = Builders<BsonDocument>.Filter.Eq("id", e.Id);
            var update = Builders<BsonDocument>.Update
                .Set("address", e.NewAddress)
                .CurrentDate("lastModified");

            collection.UpdateOne(filter, update);

            _channel.Send(ExchangeConfiguration.SuccessExchange, e, e.Metadata.ConnectionToken);
        }
    }
}
