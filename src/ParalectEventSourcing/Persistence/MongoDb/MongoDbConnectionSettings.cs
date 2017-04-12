namespace ParalectEventSourcing.Persistence.MongoDb
{
    public class MongoDbConnectionSettings
    {
        public string HostName { get; set; }
        public int Port { get; set; }

        public MongoDbConnectionSettings()
        {
#if DEBUG
            HostName = "localhost";
            Port = 27017;
#else
            HostName = "mongo";
            Port = 27017;
#endif
        }
    }
}
