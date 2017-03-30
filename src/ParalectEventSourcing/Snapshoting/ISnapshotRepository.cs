// <copyright file="ISnapshotRepository.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Snapshoting
{
    /// <summary>
    /// Snapshots repository interface
    /// </summary>
    public interface ISnapshotRepository
    {
        /// <summary>
        /// Saves snapshot to repository
        /// </summary>
        /// <param name="snapshot">the snapshot</param>
        void Save(Snapshot snapshot);

        /// <summary>
        /// Loads snapshot from repository
        /// </summary>
        /// <param name="id">aggregate root ID</param>
        /// <returns>the snapshot</returns>
        Snapshot Load(string id);
    }
}
