namespace WriteModel.CommandHandlers
{
    using Contracts.Commands;
    using Domain;
    using ParalectEventSourcing.Events;
    using ParalectEventSourcing.Exceptions;
    using ParalectEventSourcing.Repository;

    public class ShipmentCommandsHandler : ICommandHandler
    {
        private readonly IAggregateRepository<Shipment> _shipments;

        public ShipmentCommandsHandler(IAggregateRepository<Shipment> shipments)
        {
            _shipments = shipments;
        }

        public void Handle(
            CreateShipment c)
        {
            // example of DomainValidationException
            if (c.Address.StartsWith("1"))
            {
                throw new DomainValidationException($"Can't create shipment with address {c.Address}");
            }

            _shipments.Perform(
                c.Id,
                shipment => shipment.CreateShipment(c.Id, c.Metadata, c.Address));
        }

        public void Handle(
            ChangeShipmentAddress c)
        {
            _shipments.Perform(
                c.Id,
                shipment => shipment.UpdateAddress(c.Metadata, c.NewAddress));
        }
    }
}
