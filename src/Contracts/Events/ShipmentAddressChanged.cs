namespace Contracts.Events
{
    using ParalectEventSourcing.Events;

    public class ShipmentAddressChanged : Event
    {
        public string NewAddress { get; set; }
    }
}
