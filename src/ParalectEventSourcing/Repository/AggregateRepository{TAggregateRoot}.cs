// <copyright file="AggregateRepository{TAggregateRoot}.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Commands;
    using Events;
    using EventStore;
    using Snapshoting;
    using Utils;

    /// <summary>
    /// Aggregate repository
    /// </summary>
    /// <typeparam name="TAggregateRoot">Type of aggregate root</typeparam>
    public class AggregateRepository<TAggregateRoot> : IAggregateRepository<TAggregateRoot>
        where TAggregateRoot : AggregateRoot, IAggregateRoot, new()
    {
        private const int SnapshotsInterval = 10;
        private readonly IEventSource eventSource;
        private readonly IEventBus eventBus;
        private readonly ISnapshotRepository snapshots;

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRepository{TAggregateRoot}"/> class.
        /// </summary>
        /// <param name="eventSource">event source</param>
        /// <param name="eventBus">event bus</param>
        /// <param name="snapshots">snapshots repository</param>
        public AggregateRepository(IEventSource eventSource, IEventBus eventBus, ISnapshotRepository snapshots)
        {
            this.eventSource = eventSource;
            this.eventBus = eventBus;
            this.snapshots = snapshots;
        }

        /// <summary>
        /// Creates a new AR instance by invoking the AR's parameterless constructor
        /// </summary>
        /// <returns>blank AR instance</returns>
        public TAggregateRoot Create()
        {
            return new TAggregateRoot();
        }

        /// <summary>
        /// Fetches all events from the AR's stream and re-hydrates the AR
        /// using all events
        /// </summary>
        /// <param name="id">AR id</param>
        /// <returns>AR instance</returns>
        public TAggregateRoot Get(string id)
        {
            var streamId = id.ToString();
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException(string.Format(
                    "Aggregate ID was not specified when trying to get by id {0} aggregate", typeof(TAggregateRoot).FullName));
            }

            var snapshot = this.snapshots.Load(streamId);
            var stream = snapshot == null ? this.eventSource.GetEventsStream(streamId) : this.eventSource.GetEventsStream(streamId, snapshot.StreamVersion + 1);
            var aggregate = new TAggregateRoot();
            var state = snapshot != null ? snapshot.Payload : CreateAggregateState(typeof(TAggregateRoot));
            StateSpooler.Spool(state ?? aggregate, stream.Events);
            aggregate.Setup(state, stream.End);
            return aggregate;
        }

        /// <summary>
        /// Commits events to event store and event bus
        /// </summary>
        /// <param name="id">aggregate ID</param>
        /// <param name="aggregateRoot">aggregate</param>
        /// <param name="metadata">metadata</param>
        /// <returns>async task boid result</returns>
        public async Task CommitEventsAsync(string id, IAggregateRoot aggregateRoot, ICommandMetadata metadata)
        {
            var events = this.ExtractEvents(aggregateRoot, metadata).ToList();
            var version = aggregateRoot.Version;
            events.ForEach(e => e.Version = version);
            await this.eventSource.AppendEventsAsync(id.ToString(), version, events);
            if (this.eventBus != null)
            {
                this.eventBus.Publish(events);
            }

            if (aggregateRoot.Version % SnapshotsInterval == 0)
            {
                this.snapshots.Save(new Snapshot(id.ToString(), version, aggregateRoot.State));
            }
        }

        /// <summary>
        /// Commits events to event store and event bus
        /// </summary>
        /// <param name="id">aggregate ID</param>
        /// <param name="aggregateRoot">aggregate</param>
        /// <param name="metadata">metadata</param>
        public void CommitEvents(string id, IAggregateRoot aggregateRoot, ICommandMetadata metadata)
        {
            this.CommitEventsAsync(id, aggregateRoot, metadata).Wait();
        }

        /// <summary>
        /// Performs operation on the aggregate
        /// </summary>
        /// <param name="id">aggregate root ID</param>
        /// <param name="update">operation action</param>
        /// <param name="metadata">metadata</param>
        public void Perform(string id, Action<TAggregateRoot> update, ICommandMetadata metadata = null)
        {
            var aggregate = this.Get(id);
            update(aggregate);
            this.CommitEvents(id, aggregate, metadata);
        }

        /// <summary>
        /// Will return null, if cannot find aggregate state type
        /// </summary>
        /// <returns>new aggregate state</returns>
        /// <param name="aggregateType">aggregate root type</param>
        private static object CreateAggregateState(Type aggregateType)
        {
            if (aggregateType.GetTypeInfo().BaseType == null)
            {
                return null;
            }

            var aggregateInterface = aggregateType.GetTypeInfo().BaseType;
            var args = aggregateInterface.GetGenericArguments();

            if (args.Length == 0)
            {
                return null;
            }

            var aggregateStateType = args[0];
            var state = Activator.CreateInstance(aggregateStateType);

            return state;
        }

        private IEnumerable<IEvent> ExtractEvents(IAggregateRoot aggregate, ICommandMetadata metadata)
        {
            var currentTime = DateTime.UtcNow;
            foreach (var e in aggregate.UncommittedEvents)
            {
                e.Metadata.EventId = Guid.NewGuid().ToString();
                e.Metadata.StoredDate = currentTime;
                e.Metadata.TypeName = e.GetType().Name;

                // Take some metadata properties from command
                if (metadata != null)
                {
                    e.Metadata.CommandId = metadata.CommandId;
                    e.Metadata.UserId = metadata.UserId;
                }

                yield return e;
            }
        }
    }
}