namespace WriteModel
{
    using System;
    using System.Reflection;
    using System.Text;
    using CommandHandlers;
    using Domain;
    using EventHandlers;
    using Infrastructure;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using ParalectEventSourcing.Dispatching;
    using ParalectEventSourcing.Events;
    using ParalectEventSourcing.Exceptions;
    using ParalectEventSourcing.InMemory;
    using ParalectEventSourcing.Repository;
    using ParalectEventSourcing.Repository.EventStore;
    using ParalectEventSourcing.Serialization;
    using ParalectEventSourcing.Snapshoting;
    using ParalectEventSourcing.Utils;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using Serilog;

    public class Program
    {
        private const string WriteModelQueue = "WriteModelQueue";
        private const string ErrorQueue = "ErrorQueue";
        private const string HostName = "localhost";

        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory { HostName = HostName };

        private static IServiceProvider _serviceProvider;

        public static void Main(string[] args)
        {
            RegisterDependencies();
            ListenToMessages();
        }

        private static void RegisterDependencies()
        {
            var dispatcherConfiguration = new DispatcherConfiguration();
            var eventStoreConnectionsSettings = new EventStoreConnectionSettings
            {
                Host = "localhost",
                Port = 1113,
                Login = "admin",
                Pass = "changeit"
            }; // todo read from configuration

            _serviceProvider = new ServiceCollection()

                .AddTransient<IEventBus, RabbitMqEventBus>()

                .AddSingleton<ICommandDispatcher, CommandDispatcher>()
                .AddSingleton<IEventDispatcher, EventDispatcher>()

                .AddSingleton<DeviceCommandsHandler, DeviceCommandsHandler>()
                .AddSingleton<ShipmentCommandsHandler, ShipmentCommandsHandler>()
                .AddSingleton<DeviceEventsHandler, DeviceEventsHandler>()
                .AddSingleton<ShipmentEventsHandler, ShipmentEventsHandler>()

                .AddTransient<IDateTimeProvider, DateTimeProvider>()

                .AddTransient<IAggregateRepository<Device>, AggregateRepository<Device>>()
                .AddTransient<IAggregateRepository<Shipment>, AggregateRepository<Shipment>>()

                .AddTransient<IEventSource, EventSource>()
                .AddTransient<IEventStoreSerializer, DefaultEventStoreSerializer>()
                .AddSingleton(eventStoreConnectionsSettings)

                .AddTransient<ISnapshotRepository, InMemorySnapshotRepository>()
                .AddSingleton(dispatcherConfiguration)
                .AddSingleton(Log.Logger)
                .BuildServiceProvider();

            dispatcherConfiguration.ServiceLocator = _serviceProvider;

            var assemblyWithHandlers = typeof(Program).GetTypeInfo().Assembly;
            dispatcherConfiguration
                .DispatcherCommandHandlerRegistry
                .Register(assemblyWithHandlers, new[] { typeof(DeviceCommandsHandler).Namespace });
            dispatcherConfiguration
                .DispatcherEventHandlerRegistry
                .Register(assemblyWithHandlers, new[] { typeof(DeviceEventsHandler).Namespace });
        }

        private static void ListenToMessages()
        {
            using (var connection = ConnectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: WriteModelQueue,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += ConsumerOnReceived;
                channel.BasicConsume(queue: WriteModelQueue,
                                     noAck: true,
                                     consumer: consumer);

                Console.Read();
            }
        }

        private static void ConsumerOnReceived(object sender, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            var body = Encoding.UTF8.GetString(basicDeliverEventArgs.Body);
            Console.WriteLine("Received: " + body);

            var message = JsonConvert.DeserializeObject(body);

            var typeName = ((dynamic)message).Metadata.TypeName.ToString();
            var messageType = Type.GetType(typeName);
            var typedMessage = JsonConvert.DeserializeObject(body, messageType);

            try
            {
                var commandDispatcher = (dynamic) _serviceProvider.GetService<ICommandDispatcher>();
                commandDispatcher.Dispatch(typedMessage);

                Console.WriteLine("Command handled successfully.");
            }
            catch (DomainValidationException e)
            {
                SendMessage(ErrorQueue, e.Message);
            }
        }

        private static void SendMessage(string queue, string message)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queue,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                 routingKey: queue,
                                 basicProperties: null,
                                 body: body);

                Console.WriteLine("Sent: " + message);
            }
        }
    }
}
