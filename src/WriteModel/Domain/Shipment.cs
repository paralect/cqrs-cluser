namespace WriteModel.Domain
{
    using Contracts.Events;
    using ParalectEventSourcing;

    public class Shipment : Aggregate<ShipmentState>
    {
        public void UpdateAddress(string newAddress)
        {
            Apply(new ShipmentAddressChanged
            {
                Id = State.Id,
                NewAddress = newAddress
            });
        }

        public void CreateShipment(string id, string address)
        {
            Apply(new ShipmentCreated
            {
                Id = id,
                Address = address
            });
        }
    }
}
