namespace Contracts.Events
{
    using ParalectEventSourcing.Events;

    public class ShipmentAddressUpdated : Event
    {
        public string NewAddress { get; set; }
    }
}
