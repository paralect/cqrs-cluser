namespace ParalectEventSourcing.Messaging.RabbitMq
{
    using System;
    using RabbitMQ.Client.Events;

    public interface IChannel
    {
        void SendToExchange(string exchange, string routingKey, object message);

        void SendToQueue(string queue, object message);

        string SubscribeToExchange(string exchange, string routingKey, EventHandler<BasicDeliverEventArgs> callback);

        void SubscribeToQueue(string queue, EventHandler<BasicDeliverEventArgs> callback);

        void Unsubscribe(string consumerTag);
    }
}