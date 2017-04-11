namespace WriteModel.Domain
{
    using System;
    using Contracts.Events;
    using ParalectEventSourcing;
    using ParalectEventSourcing.Events;

    public class Shipment : Aggregate<ShipmentState>
    {
        public void CreateShipment(string aggregateRootId, string commandId, string address)
        {
            Apply(new ShipmentCreated
            {
                Id = aggregateRootId,
                Address = address,
                Metadata = new EventMetadata
                {
                    EventId = Guid.NewGuid().ToString(),
                    CommandId = commandId,
                    TypeName = typeof(ShipmentCreated).AssemblyQualifiedName
                }
            });
        }

        public void UpdateAddress(string commandId, string newAddress)
        {
            Apply(new ShipmentAddressChanged
            {
                Id = State.Id,
                NewAddress = newAddress,
                Metadata = new EventMetadata
                {
                    EventId = Guid.NewGuid().ToString(),
                    CommandId = commandId,
                    TypeName = typeof(ShipmentCreated).AssemblyQualifiedName
                }
            });
        }
    }
}
