// <copyright file="IAggregateRepository{TAggregate}.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Repository
{
    using System;
    using System.Threading.Tasks;
    using Commands;

    /// <summary>
    /// Aggregate reporitory interface
    /// </summary>
    /// <typeparam name="TAggregate">aggregate root type</typeparam>
    public interface IAggregateRepository<TAggregate>
        where TAggregate : IAggregateRoot, new()
    {
        /// <summary>
        /// Creates a new AR instance by invoking the AR's parameterless constructor
        /// </summary>
        /// <returns>blank AR instance</returns>
        TAggregate Create();

        /// <summary>
        /// Fetches all events from the AR's stream and re-hydrates the AR
        /// using all events
        /// </summary>
        /// <param name="id">AR id</param>
        /// <returns>AR instance</returns>
        TAggregate Get(string id);

        /// <summary>
        /// Commits events to event store and event bus
        /// </summary>
        /// <param name="id">aggregate ID</param>
        /// <param name="ar">aggregate</param>
        /// <param name="metadata">metadata</param>
        void CommitEvents(string id, IAggregateRoot ar, ICommandMetadata metadata);

        /// <summary>
        /// Commits events to event store and event bus
        /// </summary>
        /// <param name="id">aggregate ID</param>
        /// <param name="ar">aggregate</param>
        /// <param name="metadata">metadata</param>
        /// <returns>async task boid result</returns>
        Task CommitEventsAsync(string id, IAggregateRoot ar, ICommandMetadata metadata);

        /// <summary>
        /// Performs operation on the aggregate
        /// </summary>
        /// <param name="id">aggregate root ID</param>
        /// <param name="update">operation action</param>
        /// <param name="metadata">metadata</param>
        void Perform(string id, Action<TAggregate> update, ICommandMetadata metadata = null);
    }
}
