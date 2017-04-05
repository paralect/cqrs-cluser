namespace ReadModel.EventHandlers
{
    using Contracts.Events;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using ParalectEventSourcing.Events;

    public class ShipmentEventsHandler : IEventHandler
    {
        private readonly IMongoClient _client;

        public ShipmentEventsHandler(IMongoClient client)
        {
            _client = client;
        }

        public void Handle(DeviceAddedToShipment e)
        {
            
        }

        public void Handle(ShipmentCreated e)
        {
            var database = _client.GetDatabase("ReadModel");
            var collection = database.GetCollection<BsonDocument>("shipments");
            var shipment = new BsonDocument
            {
                { "id", e.Id },
                { "address", e.Address }
            };

            collection.InsertOne(shipment);
        }

        public void Handle(ShipmentAddressChanged e)
        {
            var database = _client.GetDatabase("ReadModel");
            var collection = database.GetCollection<BsonDocument>("shipments");
            var filter = Builders<BsonDocument>.Filter.Eq("id", e.Id);
            var update = Builders<BsonDocument>.Update
                .Set("address", e.NewAddress)
                .CurrentDate("lastModified");

            var result = collection.UpdateOneAsync(filter, update).Result;
        }
    }
}
