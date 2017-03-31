namespace WriteModel
{
    using System;
    using System.Reflection;
    using System.Text;
    using CommandHandlers;
    using Domain;
    using EventHandlers;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using ParalectEventSourcing.Commands;
    using ParalectEventSourcing.Dispatching;
    using ParalectEventSourcing.Events;
    using ParalectEventSourcing.InMemory;
    using ParalectEventSourcing.Repository;
    using ParalectEventSourcing.Repository.EventStore;
    using ParalectEventSourcing.Snapshoting;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using Serilog;

    public class Program
    {
        private static IServiceProvider _serviceProvider;

        public static void Main(string[] args)
        {
            RegisterDependencies();
            ListenToMessages();
        }

        private static void RegisterDependencies()
        {
            var dispatcherConfiguration = new DispatcherConfiguration();

            _serviceProvider = new ServiceCollection()
                .AddTransient<ICommandBus, CommandBus>()
                .AddTransient<IEventBus, EventBus>()

                .AddSingleton<ICommandDispatcher, CommandDispatcher>()
                .AddSingleton<IEventDispatcher, EventDispatcher>()

                .AddSingleton<DeviceCommandsHandler, DeviceCommandsHandler>()
                .AddSingleton<ShipmentCommandsHandler, ShipmentCommandsHandler>()
                .AddSingleton<DeviceEventsHandler, DeviceEventsHandler>()
                .AddSingleton<ShipmentEventsHandler, ShipmentEventsHandler>()

                .AddTransient<IDateTimeProvider, DateTimeProvider>()

                .AddTransient<IAggregateRepository<Device>, AggregateRepository<Device>>()
                .AddTransient<IAggregateRepository<Shipment>, AggregateRepository<Shipment>>()
                .AddTransient<IEventSource, InMemoryEventSource>()

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
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += ConsumerOnReceived;
                channel.BasicConsume(queue: "hello",
                                     noAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static void ConsumerOnReceived(object sender, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            var body = Encoding.UTF8.GetString(basicDeliverEventArgs.Body);
            var message = JsonConvert.DeserializeObject(body);
            Console.WriteLine(" [x] Received {0}", message);

            var typeName = ((dynamic)message).Metadata.TypeName.ToString();
            var messageType = Type.GetType(typeName);
            var typedMessage = JsonConvert.DeserializeObject(body, messageType);

            var commandBus = _serviceProvider.GetService<ICommandBus>();
            commandBus.Send(typedMessage);
        }
    }
}
