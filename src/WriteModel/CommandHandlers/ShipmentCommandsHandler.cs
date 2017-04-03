namespace WriteModel.CommandHandlers
{
    using Contracts.Commands;
    using Domain;
    using ParalectEventSourcing.Events;
    using ParalectEventSourcing.Repository;

    public class ShipmentCommandsHandler : ICommandHandler
    {
        private readonly IAggregateRepository<Shipment> _shipments;

        public ShipmentCommandsHandler(IAggregateRepository<Shipment> shipments)
        {
            _shipments = shipments;
        }

        public void Handle(
            UpdateShipmentAddress c)
        {
            _shipments.Perform(
                c.Id,
                shipment => shipment.UpdateAddress(
                    c.NewAddress));
        }
    }
}
