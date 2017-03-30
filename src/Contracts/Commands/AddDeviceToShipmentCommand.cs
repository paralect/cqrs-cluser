namespace Contracts.Commands
{
    using ParalectEventSourcing.Commands;

    public class AddDeviceToShipmentCommand : Command
    {
        public string ShipmentKey { get; set; }
    }
}
