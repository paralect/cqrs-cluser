namespace Contracts.Commands
{
    using ParalectEventSourcing.Commands;

    public class UpdateShipmentAddressCommand : Command
    {
        public string NewAddress { get; set; }
    }
}
