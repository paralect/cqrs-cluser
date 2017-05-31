// <copyright file="AggregateRepository{TAggregateRoot}.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

using ParalectEventSourcing.Serialization;

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
        private readonly IEventSource _eventSource;
        private readonly IEventBus _eventBus;
        private readonly ISnapshotRepository _snapshots;
        private readonly IMessageSerializer _messageSerializer;

        public AggregateRepository(IEventSource eventSource, IEventBus eventBus, ISnapshotRepository snapshots, IMessageSerializer messageSerializer)
        {
            _eventSource = eventSource;
            _eventBus = eventBus;
            _snapshots = snapshots;
            _messageSerializer = messageSerializer;
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
            var streamId = id;
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException(string.Format(
                    "Aggregate ID was not specified when trying to get by id {0} aggregate", typeof(TAggregateRoot).FullName));
            }

            var snapshot = _snapshots.Load(streamId);
            var aggregateStateType = GetAggregateStateType(typeof(TAggregateRoot));

            object state;
            EventsStream stream;
            if (snapshot == null)
            {
                state = CreateAggregateState(aggregateStateType);
                stream = _eventSource.GetEventsStream(streamId);
            }
            else
            {
                state = _messageSerializer.Deserialize(snapshot.Payload, aggregateStateType);
                stream = _eventSource.GetEventsStream(streamId, snapshot.StreamVersion + 1);
            }

            var aggregate = new TAggregateRoot();
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
            var events = ExtractEvents(aggregateRoot, metadata).ToList();
            var version = aggregateRoot.Version;
            events.ForEach(e => e.Version = version);
            await _eventSource.AppendEventsAsync(id, version, events);
            _eventBus?.Publish(events);

            if (aggregateRoot.Version % SnapshotsInterval == 0)
            {
                _snapshots.Save(new Snapshot(id, version, _messageSerializer.Serialize(aggregateRoot.State)));
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
            CommitEventsAsync(id, aggregateRoot, metadata).Wait();
        }

        /// <summary>
        /// Performs operation on the aggregate
        /// </summary>
        /// <param name="id">aggregate root ID</param>
        /// <param name="update">operation action</param>
        /// <param name="metadata">metadata</param>
        public void Perform(string id, Action<TAggregateRoot> update, ICommandMetadata metadata = null)
        {
            var aggregate = Get(id);
            update(aggregate);
            CommitEvents(id, aggregate, metadata);
        }

        private static object CreateAggregateState(Type aggregateStateType)
        {
            return Activator.CreateInstance(aggregateStateType);
        }

        private static Type GetAggregateStateType(Type aggregateType)
        {
            if (aggregateType.GetTypeInfo().BaseType == null)
            {
                return null;
            }

            var aggregateInterface = aggregateType.GetTypeInfo().BaseType;
            var args = aggregateInterface.GetGenericArguments();

            return args.Length == 0 ? null : args[0];
        }

        private IEnumerable<IEvent> ExtractEvents(IAggregateRoot aggregate, ICommandMetadata metadata)
        {
            var currentTime = DateTime.UtcNow;
            foreach (var e in aggregate.UncommittedEvents)
            {
                e.Metadata.EventId = Guid.NewGuid().ToString();
                e.Metadata.StoredDate = currentTime;
                e.Metadata.TypeName = e.GetType().AssemblyQualifiedName;

                // Take some metadata properties from command
                if (metadata != null)
                {
                    e.Metadata.CommandId = metadata.CommandId;
                    e.Metadata.ConnectionId = metadata.ConnectionId;
                    e.Metadata.UserId = metadata.UserId;
                }

                yield return e;
            }
        }
    }
}