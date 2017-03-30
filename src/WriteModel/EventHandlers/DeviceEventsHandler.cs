namespace WriteModel.EventHandlers
{
    using Contracts.Events;
    using ParalectEventSourcing.Events;

    public class DeviceEventsHandler : IEventHandler
    {
        public void Handle(DeviceAddedToShipment e)
        {
            // Update read model here
        }
    }
}
