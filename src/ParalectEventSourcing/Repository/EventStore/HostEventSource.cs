namespace ParalectEventSourcing.Repository.EventStore
{
    using global::EventStore.ClientAPI;
    using Microsoft.Extensions.Options;
    using Serialization;
    using Utils;

    public class HostEventSource : EventSource
    {
        public HostEventSource(IOptions<EventStoreConnectionSettings> connectionSettingsAccessor, IEventStoreSerializer serializer) 
            : base(connectionSettingsAccessor.Value, serializer)
        {
            var connectionSettings = connectionSettingsAccessor.Value;
            var connectionSettingsBuilder = ConnectionSettings.Create().KeepReconnecting();
            var point = IpEndPointUtility.CreateIpEndPoint(connectionSettings.Host + ":" + connectionSettings.Port).Result;
            Connection = EventStoreConnection.Create(connectionSettingsBuilder, point);

            Connection.ConnectAsync().Wait();
        }
    }
}
