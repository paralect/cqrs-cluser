// <copyright file="ISnapshotRepository.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Snapshoting
{
    using System.Threading.Tasks;

    public interface ISnapshotRepository
    {
        Task SaveAsync(Snapshot snapshot);

        Task<Snapshot> LoadAsync(string id);
    }
}
