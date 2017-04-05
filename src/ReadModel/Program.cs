namespace ReadModel
{
    using System;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using EventHandlers;
    using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Driver;
    using Newtonsoft.Json;
    using ParalectEventSourcing.Dispatching;
    using ParalectEventSourcing.Messaging.RabbitMq;
    using ParalectEventSourcing.Persistence;
    using ParalectEventSourcing.Serialization;
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
            var rabbitMqConnectionSettings = new RabbitMqConnectionSettings
            {
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                HostName = "localhost",
                Port = 5672
            }; // TODO read from configuration

            var dispatcherConfiguration = new DispatcherConfiguration();

            _serviceProvider = new ServiceCollection()

                // TODO consider creating channels per thread
                .AddTransient<IChannel, Channel>()
                .AddSingleton<RabbitMqConnectionSettings>(rabbitMqConnectionSettings)
                .AddSingleton<IChannelFactory, ChannelFactory>()
                .AddTransient<IMessageSerializer, DefaultMessageSerializer>()

                .AddTransient<IEventDispatcher, EventDispatcher>()

                .AddSingleton<DeviceEventsHandler, DeviceEventsHandler>()
                .AddSingleton<ShipmentEventsHandler, ShipmentEventsHandler>()

                .AddSingleton<DispatcherConfiguration>(dispatcherConfiguration)
                .AddSingleton<ILogger>(Log.Logger)

                .AddSingleton<IMongoClient, MongoClient>()

                .BuildServiceProvider();

            dispatcherConfiguration.ServiceLocator = _serviceProvider;

            dispatcherConfiguration
                .DispatcherEventHandlerRegistry
                .Register(Assembly.GetEntryAssembly(), new[] { typeof(DeviceEventsHandler).Namespace });
        }

        private static void ListenToMessages()
        {
            Task.Run(() =>
            {
                var channel = _serviceProvider.GetService<IChannel>();
                channel.Listen(QueueConfiguration.ReadModelQueue, ConsumerOnReceived);
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

            var eventDispatcher = (dynamic)_serviceProvider.GetService<IEventDispatcher>();
            eventDispatcher.Dispatch(typedMessage);

            Console.WriteLine("Event handled successfully.");
        }
    }
}
