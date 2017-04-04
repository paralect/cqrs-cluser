namespace ParalectEventSourcing.Messaging.RabbitMq
{
    using Commands;
    using Utils;

    public class RabbitMqCommandBus : CommandBus
    {
        private readonly IChannel _channel;

        public RabbitMqCommandBus(IDateTimeProvider dateTimeProvider, IChannel channel)
            : base(dateTimeProvider)
        {
            _channel = channel;
        }

        protected override void SendInternal(params ICommand[] commands)
        {
            foreach (var command in commands)
            {
                _channel.Send(QueueConfiguration.WriteModelQueue, command);
            }
        }
    }
}
