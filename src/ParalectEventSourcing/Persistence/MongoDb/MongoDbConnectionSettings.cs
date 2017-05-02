namespace ParalectEventSourcing.Persistence.MongoDb
{
    public class MongoDbConnectionSettings
    {
        public string ConnectionString { get; set; }

        public MongoDbConnectionSettings()
        {
#if DEBUG
            ConnectionString = "mongodb://localhost:27017";
#else
            ConnectionString = "mongodb://mongo-0.mongo,mongo-1.mongo,mongo-2.mongo:27017";
#endif
        }
    }
}
