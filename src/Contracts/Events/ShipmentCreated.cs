namespace Contracts.Events
{
    using ParalectEventSourcing.Events;

    public class ShipmentCreated : Event
    {
        public string Address { get; set; }
    }
}
