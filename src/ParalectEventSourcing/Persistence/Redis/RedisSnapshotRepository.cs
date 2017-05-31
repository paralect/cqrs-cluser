namespace ParalectEventSourcing.Persistence.Redis
{
    using Microsoft.Extensions.Caching.Distributed;
    using Serialization;
    using Snapshoting;

    public class RedisSnapshotRepository : ISnapshotRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IMessageSerializer _messageSerializer;

        public RedisSnapshotRepository(IDistributedCache distributedCache, IMessageSerializer messageSerializer)
        {
            _distributedCache = distributedCache;
            _messageSerializer = messageSerializer;
        }

        public void Save(Snapshot snapshot)
        {
            _distributedCache.Set(snapshot.StreamId, _messageSerializer.Serialize(snapshot));
        }

        public Snapshot Load(string id)
        {
            var snapshot = _distributedCache.Get(id);
            return snapshot == null ? null : _messageSerializer.Deserialize(snapshot);
        }
    }
}