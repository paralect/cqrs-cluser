namespace ParalectEventSourcing.Persistence.Redis
{
    using Microsoft.Extensions.Caching.Distributed;
    using Serialization;
    using Snapshoting;

    public class RedisSnapshotRepository : ISnapshotRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ISerializer _serializer;

        public RedisSnapshotRepository(IDistributedCache distributedCache, ISerializer serializer)
        {
            _distributedCache = distributedCache;
            _serializer = serializer;
        }

        public void Save(Snapshot snapshot)
        {
            _distributedCache.Set(snapshot.StreamId, _serializer.Serialize(snapshot));
        }

        public Snapshot Load(string id)
        {
            var snapshot = _distributedCache.Get(id);
            return snapshot == null ? null : _serializer.Deserialize(snapshot, typeof(Snapshot));
        }
    }
}