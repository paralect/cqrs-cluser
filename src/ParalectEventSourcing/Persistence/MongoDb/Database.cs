namespace ParalectEventSourcing.Persistence.MongoDb
{
    using MongoDB.Bson;
    using MongoDB.Driver;

    public class Database : IDatabase
    {
        private const string DatabaseName = "ReadModel";
        private readonly IMongoDatabase _database;

        public Database(IMongoClient client)
        {
            _database = client.GetDatabase(DatabaseName);
        }

        public IMongoCollection<BsonDocument> GetCollection(string name)
        {
            return _database.GetCollection<BsonDocument>(name);
        }
    }
}
