namespace ReadModel.EventHandlers
{
    using Contracts.Events;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using ParalectEventSourcing.Events;
    using ParalectEventSourcing.Persistence.MongoDb;

    public class ShipmentEventsHandler : IEventHandler
    {
        private const string ShipmentCollection = "shipments";

        private readonly IDatabase _database;

        public ShipmentEventsHandler(IDatabase database)
        {
            _database = database;
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
        }

        public void Handle(ShipmentAddressChanged e)
        {
            var collection = _database.GetCollection(ShipmentCollection);
            var filter = Builders<BsonDocument>.Filter.Eq("id", e.Id);
            var update = Builders<BsonDocument>.Update
                .Set("address", e.NewAddress)
                .CurrentDate("lastModified");

            var result = collection.UpdateOne(filter, update);
        }
    }
}
