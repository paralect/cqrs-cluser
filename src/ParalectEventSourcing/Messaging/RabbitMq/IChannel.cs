namespace ParalectEventSourcing.Messaging.RabbitMq
{
    using System;
    using RabbitMQ.Client.Events;

    public interface IChannel
    {
        void Send(string exchange, object message, string routingKey = "");

        string Subscribe(string exchange, EventHandler<BasicDeliverEventArgs> callback, string routingKey = "");

        void Unsubscribe(string consumerTag);
    }
}