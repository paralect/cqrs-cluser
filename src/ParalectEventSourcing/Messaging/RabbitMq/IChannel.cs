namespace ParalectEventSourcing.Messaging.RabbitMq
{
    using System;
    using RabbitMQ.Client.Events;

    public interface IChannel
    {
        void Send(string exchange, object message, string routingKey = "");

        void Listen(string exchange, EventHandler<BasicDeliverEventArgs> callback, string routingKey = "");
    }
}