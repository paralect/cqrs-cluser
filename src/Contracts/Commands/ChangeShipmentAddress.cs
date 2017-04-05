namespace Contracts.Commands
{
    using ParalectEventSourcing.Commands;

    public class ChangeShipmentAddress : Command
    {
        public string NewAddress { get; set; }
    }
}
