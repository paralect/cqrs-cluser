namespace WriteModel.Domain
{
    using System;
    using Contracts.Events;
    using ParalectEventSourcing;

    public class Device : Aggregate<DeviceState>
    {
        public void AddToShipment(
            string shipmentKey)
        {
            Apply(
                new DeviceAddedToShipment
                {
                    ShipmentKey = shipmentKey,
                    Id = Guid.NewGuid().ToString()
                });
        }
    }
}
