namespace WriteModel.Domain
{
    using Contracts.Events;

    public class ShipmentState
    {
        public string Id { get; set; }
        public string Address { get; set; }

        public void On(ShipmentCreated e)
        {
            Id = e.Id;
            Address = e.Address;
        }

        public void On(ShipmentAddressChanged e)
        {
            Address = e.NewAddress;
        }
    }
}
