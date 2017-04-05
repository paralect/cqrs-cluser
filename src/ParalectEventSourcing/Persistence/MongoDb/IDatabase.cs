namespace ParalectEventSourcing.Persistence.MongoDb
{
    using MongoDB.Bson;
    using MongoDB.Driver;

    public interface IDatabase
    {
        IMongoCollection<BsonDocument> GetCollection(string name);
    }
}
