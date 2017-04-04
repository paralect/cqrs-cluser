namespace ParalectEventSourcing.Messaging
{
    using System;
    using RabbitMQ.Client.Events;

    public interface IChannel
    {
        void Send(string queue, object message);

        void Listen(string queue, EventHandler<BasicDeliverEventArgs> callback);
    }
}