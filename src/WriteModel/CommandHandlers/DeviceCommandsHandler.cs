namespace WriteModel.CommandHandlers
{
    using Contracts.Commands;
    using Domain;
    using ParalectEventSourcing.Events;
    using ParalectEventSourcing.Repository;

    /// <summary>
    /// Command handler for Device aggegate
    /// </summary>
    public class DeviceCommandsHandler : ICommandHandler
    {
        private readonly IAggregateRepository<Device> _devices;

        public DeviceCommandsHandler(IAggregateRepository<Device> devices)
        {
            _devices = devices;
        }

        public void Handle(
            AddDeviceToShipment c)
        {
            _devices.Perform(
                c.Id,
                device => device.AddToShipment(
                    c.ShipmentKey));
        }
    }
}
