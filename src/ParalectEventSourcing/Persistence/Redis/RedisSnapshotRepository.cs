namespace ParalectEventSourcing.Persistence.Redis
{
    using System;
    using System.Threading.Tasks;
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

        public async Task SaveAsync(Snapshot snapshot)
        {
            try
            {
                await _distributedCache.SetAsync(snapshot.StreamId, _serializer.Serialize(snapshot)).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Can't save the snapshot with ID {snapshot.StreamId}: {e.Message}");
            }
        }

        public async Task<Snapshot> LoadAsync(string id)
        {
            byte[] snapshot = null;
            try
            {
                snapshot = await _distributedCache.GetAsync(id).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Can't get the snapshot with ID {id}: {e.Message}");
            }

            return snapshot == null ? null : _serializer.Deserialize(snapshot, typeof(Snapshot));
        }
    }
}