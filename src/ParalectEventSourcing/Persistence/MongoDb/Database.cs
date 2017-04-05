namespace ParalectEventSourcing.Persistence.MongoDb
{
    using MongoDB.Bson;
    using MongoDB.Driver;

    public class Database : IDatabase
    {
        private readonly IMongoDatabase _database;

        public Database(IMongoClient client, MongoDbConnectionSettings connectionSettings)
        {
            _database = client.GetDatabase(connectionSettings.DatabaseName);
        }

        public IMongoCollection<BsonDocument> GetCollection(string name)
        {
            return _database.GetCollection<BsonDocument>(name);
        }
    }
}
