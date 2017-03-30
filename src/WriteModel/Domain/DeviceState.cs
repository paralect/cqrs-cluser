namespace WriteModel.Domain
{
    using Contracts.Events;

    public class DeviceState
    {
        public string ShipmentKey { get; private set; }

        public void On(
            DeviceAddedToShipment e)
        {
            this.ShipmentKey = e.ShipmentKey;
        }
    }
}
