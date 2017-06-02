namespace ParalectEventSourcing.Persistence.Redis
{
    using System;
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
            try
            {
                _distributedCache.Set(snapshot.StreamId, _serializer.Serialize(snapshot));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Can't save the snapshot with ID {snapshot.StreamId}: {e.Message}");
            }
        }

        public Snapshot Load(string id)
        {
            byte[] snapshot = null;
            try
            {
                snapshot = _distributedCache.Get(id);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Can't get the snapshot with ID {id}: {e.Message}");
            }

            return snapshot == null ? null : _serializer.Deserialize(snapshot, typeof(Snapshot));
        }
    }
}