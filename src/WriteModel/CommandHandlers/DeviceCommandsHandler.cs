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
            this._devices = devices;
        }

        public void Handle(
            AddDeviceToShipmentCommand c)
        {
            this._devices.Perform(
                c.Id,
                device => device.AddToShipment(
                    c.ShipmentKey));
        }
    }
}
