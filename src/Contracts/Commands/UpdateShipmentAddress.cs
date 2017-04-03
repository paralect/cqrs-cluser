namespace Contracts.Commands
{
    using ParalectEventSourcing.Commands;

    public class UpdateShipmentAddress : Command
    {
        public string NewAddress { get; set; }
    }
}
