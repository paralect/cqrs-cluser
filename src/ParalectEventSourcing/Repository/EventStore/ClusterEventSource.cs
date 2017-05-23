namespace ParalectEventSourcing.Repository.EventStore
{
    using global::EventStore.ClientAPI;
    using Microsoft.Extensions.Options;
    using Serialization;

    public class ClusterEventSource : EventSource
    {
        public ClusterEventSource(IOptions<EventStoreConnectionSettings> connectionSettingsAccessor, IEventStoreSerializer serializer)
            : base(connectionSettingsAccessor.Value, serializer)
        {
            var connectionSettings = connectionSettingsAccessor.Value;
            var connectionSettingsBuilder = ConnectionSettings.Create().KeepReconnecting();
            Connection = EventStoreConnection.Create(
                connectionSettingsBuilder,
                ClusterSettings.Create().DiscoverClusterViaDns()
                    .SetClusterDns(connectionSettings.ClusterDns)
                    .SetClusterGossipPort(connectionSettings.GossipPort));

            Connection.ConnectAsync().Wait();
        }
    }
}
