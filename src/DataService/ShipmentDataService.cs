namespace DataService
{
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using ParalectEventSourcing.Persistence.MongoDb;

    public class ShipmentDataService : IShipmentDataService
    {
        private const string ShipmentCollection = "shipments";

        private readonly IDatabase _database;

        public ShipmentDataService(IDatabase database)
        {
            _database = database;
        }

        public IEnumerable<Shipment> GetAllShipments()
        {
            var collection = _database.GetCollection(ShipmentCollection);
            var shipments = collection.Find(Builders<BsonDocument>.Filter.Empty).ToList().Select(s => new Shipment
            {
                Id = s["id"].AsString,
                Address = s["address"].AsString
            });

            return shipments;
        }
    }
}
