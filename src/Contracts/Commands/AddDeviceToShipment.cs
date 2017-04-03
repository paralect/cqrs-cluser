namespace Contracts.Commands
{
    using ParalectEventSourcing.Commands;

    public class AddDeviceToShipment : Command
    {
        public string ShipmentKey { get; set; }
    }
}
