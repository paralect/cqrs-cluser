// <copyright file="InMemoryEventSource.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.InMemory
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Events;
    using Exceptions;
    using Repository.EventStore;

    /// <summary>
    /// Inmemory event store
    /// </summary>
    public class InMemoryEventSource : IEventSource
    {
        private readonly ConcurrentDictionary<string, Dictionary<int, Transition>> store = new ConcurrentDictionary<string, Dictionary<int, Transition>>();

        /// <inheritdoc/>
        public Task AppendEventsAsync(string streamId, int version, IEnumerable<IEvent> events)
        {
            var nextVersion = version + 1;
            var stream = this.GetStream(streamId);
            if (stream.ContainsKey(nextVersion))
            {
                throw new DuplicateTransitionException(streamId, nextVersion, null);
            }

            var transition = new Transition()
            {
                Version = nextVersion,
                Events = events.ToList()
            };
            stream.Add(nextVersion, transition);

            return Task.FromResult(false);
        }

        /// <inheritdoc/>
        public EventsStream GetEventsStream(string streamId, int offset)
        {
            var stream = this.GetStream(streamId);
            var events = stream.Values.Skip(offset).SelectMany(x => x.Events).ToList();
            return new EventsStream(streamId, offset, stream.Any() ? stream.Max(x => x.Key) : 0, events);
        }

        /// <inheritdoc/>
        public EventsStream GetEventsStream(string streamId)
        {
            return this.GetEventsStream(streamId, 0);
        }

        private Dictionary<int, Transition> GetStream(string streamId)
        {
            if (!this.store.ContainsKey(streamId))
            {
                this.store[streamId] = new Dictionary<int, Transition>();
            }

            return this.store[streamId];
        }

        internal class Transition
        {
            public int Version { get; set; }

            public List<IEvent> Events { get; set; }
        }
    }
}