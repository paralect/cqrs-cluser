namespace Contracts.Events
{
    using ParalectEventSourcing.Events;

    public class DeviceAddedToShipment : Event
    {
        public string ShipmentKey { get; set; }
    }
}
