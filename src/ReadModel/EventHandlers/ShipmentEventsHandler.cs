namespace ReadModel.EventHandlers
{
    using Contracts.Events;
    using ParalectEventSourcing.Events;

    public class ShipmentEventsHandler : IEventHandler
    {
        public void Handle(DeviceAddedToShipment e)
        {
            // Update read model here
        }

        public void Handle(ShipmentAddressChanged e)
        {
            
        }
    }
}
