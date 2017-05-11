namespace WriteModel.Domain
{
    using Contracts.Events;
    using ParalectEventSourcing;
    using ParalectEventSourcing.Commands;

    public class Shipment : Aggregate<ShipmentState>
    {
        public void CreateShipment(string aggregateRootId, ICommandMetadata commandMetadata, string address)
        {
            Apply(new ShipmentCreated
            {
                Id = aggregateRootId,
                Address = address
            });
        }

        public void UpdateAddress(ICommandMetadata commandMetadata, string newAddress)
        {
            Apply(new ShipmentAddressChanged
            {
                Id = State.Id,
                NewAddress = newAddress
            });
        }
    }
}
