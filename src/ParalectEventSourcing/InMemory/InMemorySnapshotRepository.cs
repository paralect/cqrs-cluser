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
        private readonly ConcurrentDictionary<string, Snapshot> snapshots;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemorySnapshotRepository"/> class.
        /// </summary>
        public InMemorySnapshotRepository()
        {
            this.snapshots = new ConcurrentDictionary<string, Snapshot>();
        }

        /// <inheritdoc/>
        public void Save(Snapshot snapshot)
        {
            this.snapshots[snapshot.StreamId] = snapshot;
        }

        /// <inheritdoc/>
        public Snapshot Load(string id)
        {
            return this.snapshots.ContainsKey(id) ? this.snapshots[id] : null;
        }
    }
}