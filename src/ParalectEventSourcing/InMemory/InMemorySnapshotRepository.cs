// <copyright file="InMemorySnapshotRepository.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.InMemory
{
    using System.Collections.Concurrent;
    using Snapshoting;

    /// <summary>
    /// Inmemory imitation of snapthos repository
    /// </summary>
    public class InMemorySnapshotRepository : ISnapshotRepository
    {
        private readonly ConcurrentDictionary<string, Snapshot> _snapshots;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemorySnapshotRepository"/> class.
        /// </summary>
        public InMemorySnapshotRepository()
        {
            _snapshots = new ConcurrentDictionary<string, Snapshot>();
        }

        /// <inheritdoc/>
        public void Save(Snapshot snapshot)
        {
            _snapshots[snapshot.StreamId] = snapshot;
        }

        /// <inheritdoc/>
        public Snapshot Load(string id)
        {
            return _snapshots.ContainsKey(id) ? _snapshots[id] : null;
        }
    }
}