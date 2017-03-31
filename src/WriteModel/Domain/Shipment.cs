namespace WriteModel.Domain
{
    using System;
    using Contracts.Events;
    using ParalectEventSourcing;

    public class Shipment : Aggregate<ShipmentState>
    {
        public void UpdateAddress(string newAddress)
        {
            Apply(new ShipmentAddressUpdated
            {
                NewAddress = newAddress,
                Id = Guid.NewGuid().ToString()
            });
        }
    }
}
