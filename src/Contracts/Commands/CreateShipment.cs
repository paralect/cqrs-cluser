namespace Contracts.Commands
{
    using ParalectEventSourcing.Commands;

    public class CreateShipment : Command
    {
        public string Address { get; set; }
    }
}
