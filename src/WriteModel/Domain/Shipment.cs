namespace WriteModel.Domain
{
    using System;
    using Contracts.Events;
    using ParalectEventSourcing;
    using ParalectEventSourcing.Commands;
    using ParalectEventSourcing.Events;

    public class Shipment : Aggregate<ShipmentState>
    {
        public void CreateShipment(string aggregateRootId, ICommandMetadata commandMetadata, string address)
        {
            Apply(new ShipmentCreated
            {
                Id = aggregateRootId,
                Address = address,
                Metadata = CreateEventMetadata(commandMetadata, typeof(ShipmentCreated))
            });
        }

        public void UpdateAddress(ICommandMetadata commandMetadata, string newAddress)
        {
            Apply(new ShipmentAddressChanged
            {
                Id = State.Id,
                NewAddress = newAddress,
                Metadata = CreateEventMetadata(commandMetadata, typeof(ShipmentAddressChanged))
            });
        }

        // TODO move somewhere
        private EventMetadata CreateEventMetadata(ICommandMetadata commandMetadata, Type eventType)
        {
            return new EventMetadata
            {
                EventId = Guid.NewGuid().ToString(),
                CommandId = commandMetadata.CommandId,
                ConnectionId = commandMetadata.ConnectionId,
                ConnectionToken = commandMetadata.ConnectionToken,
                TypeName = eventType.AssemblyQualifiedName
            };
        }
    }
}
