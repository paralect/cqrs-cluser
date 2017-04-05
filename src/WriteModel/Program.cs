namespace WriteModel
{
    using System;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using CommandHandlers;
    using Domain;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using ParalectEventSourcing.Dispatching;
    using ParalectEventSourcing.Events;
    using ParalectEventSourcing.Exceptions;
    using ParalectEventSourcing.InMemory;
    using ParalectEventSourcing.Messaging.RabbitMq;
    using ParalectEventSourcing.Repository;
    using ParalectEventSourcing.Repository.EventStore;
    using ParalectEventSourcing.Serialization;
    using ParalectEventSourcing.Snapshoting;
    using ParalectEventSourcing.Utils;
    using RabbitMQ.Client.Events;
    using Serilog;

    public class Program
    {
        private static IServiceProvider _serviceProvider;

        public static void Main(string[] args)
        {
            RegisterDependencies();
            ListenToMessages();

            Console.Read();
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
            }; // TODO read from configuration

            var rabbitMqConnectionSettings = new RabbitMqConnectionSettings
            {
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                HostName = "localhost",
                Port = 5672
            }; // TODO read from configuration

            _serviceProvider = new ServiceCollection()

                // TODO consider creating channels per thread
                .AddTransient<IChannel, Channel>()
                .AddSingleton<RabbitMqConnectionSettings>(rabbitMqConnectionSettings)
                .AddSingleton<IChannelFactory, ChannelFactory>()
                .AddTransient<IMessageSerializer, DefaultMessageSerializer>()

                .AddTransient<IEventBus, RabbitMqEventBus>()

                .AddSingleton<IDispatcher, CommandDispatcher>()

                .AddSingleton<DeviceCommandsHandler, DeviceCommandsHandler>()
                .AddSingleton<ShipmentCommandsHandler, ShipmentCommandsHandler>()
            
                .AddTransient<IDateTimeProvider, DateTimeProvider>()

                .AddTransient<IAggregateRepository<Device>, AggregateRepository<Device>>()
                .AddTransient<IAggregateRepository<Shipment>, AggregateRepository<Shipment>>()

                .AddTransient<IEventSource, EventSource>()
                .AddTransient<IEventStoreSerializer, MessagePackEventStoreSerializer>()
                .AddSingleton<EventStoreConnectionSettings>(eventStoreConnectionsSettings)

                .AddTransient<ISnapshotRepository, InMemorySnapshotRepository>()

                .AddSingleton<DispatcherConfiguration>(dispatcherConfiguration)
                .AddSingleton<ILogger>(Log.Logger)
                .BuildServiceProvider();

            dispatcherConfiguration.ServiceLocator = _serviceProvider;

            dispatcherConfiguration
                .DispatcherCommandHandlerRegistry
                .Register(Assembly.GetEntryAssembly(), new[] { typeof(DeviceCommandsHandler).Namespace });
        }

        private static void ListenToMessages()
        {
            Task.Run(() =>
            {
                var channel = _serviceProvider.GetService<IChannel>();
                channel.Listen(QueueConfiguration.WriteModelQueue, ConsumerOnReceived);
            });
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
                var commandDispatcher = _serviceProvider.GetService<IDispatcher>();
                commandDispatcher.Dispatch(typedMessage);

                Console.WriteLine("Command handled successfully.");
            }
            catch (DomainValidationException e)
            {
                var channel = _serviceProvider.GetService<IChannel>();
                channel.Send(QueueConfiguration.ErrorQueue, e.Message);
            }
        }
    }
}
