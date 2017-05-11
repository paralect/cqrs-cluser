namespace ReadModel
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using EventHandlers;
    using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Driver;
    using ParalectEventSourcing.Dispatching;
    using ParalectEventSourcing.Messaging.RabbitMq;
    using ParalectEventSourcing.Persistence.MongoDb;
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

            Console.ReadLine();
        }

        private static void RegisterDependencies()
        {
            var dispatcherConfiguration = new DispatcherConfiguration();

            _serviceProvider = new ServiceCollection()

                .AddTransient<IMessageSerializer, DefaultMessageSerializer>()

                .AddTransient<RabbitMqConnectionSettings, RabbitMqConnectionSettings>()
                .AddSingleton<IChannelFactory, ChannelFactory>()
                .AddSingleton<IReadModelChannel>(sp => sp.GetService<IChannelFactory>().CreateChannel())
                .AddSingleton<ISuccessChannel>(sp => sp.GetService<IChannelFactory>().CreateChannel())

                .AddTransient<IDispatcher, EventDispatcher>()

                .AddSingleton<ShipmentEventsHandler, ShipmentEventsHandler>()

                .AddSingleton<DispatcherConfiguration>(dispatcherConfiguration)
                .AddSingleton<ILogger>(Log.Logger)

                .AddSingleton<IMongoClient>(new MongoClient(new MongoDbConnectionSettings().ConnectionString))
                .AddTransient<IDatabase, Database>()

                .BuildServiceProvider();

            dispatcherConfiguration.ServiceLocator = _serviceProvider;

            dispatcherConfiguration
                .DispatcherEventHandlerRegistry
                .Register(Assembly.GetEntryAssembly(), new[] { typeof(ShipmentEventsHandler).Namespace });
        }

        private static void ListenToMessages()
        {
            Task.Run(() =>
            {
                var channel = _serviceProvider.GetService<IReadModelChannel>();
                channel.SubscribeToQueue(RabbitMqRoutingConfiguration.ReadModelQueue, ConsumerOnReceived);
            });
        }

        private static void ConsumerOnReceived(object sender, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            var messageSerializer = _serviceProvider.GetService<IMessageSerializer>();
            var @event = messageSerializer.Deserialize(basicDeliverEventArgs.Body, e => e.Metadata.TypeName);

            var eventDispatcher = _serviceProvider.GetService<IDispatcher>();
            eventDispatcher.Dispatch(@event);

            Console.WriteLine($"Event {@event.Metadata.EventId} handled successfully.");
        }
    }
}
