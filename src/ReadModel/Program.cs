namespace ReadModel
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using EventHandlers;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Driver;
    using ParalectEventSourcing.Dispatching;
    using ParalectEventSourcing.Messaging.RabbitMq;
    using ParalectEventSourcing.Persistence.MongoDb;
    using ParalectEventSourcing.Serialization;
    using RabbitMQ.Client.Events;
    using Serilog;
    using System.IO;
    using Microsoft.Extensions.Options;

    public class Program
    {
        private static IServiceProvider _serviceProvider;

        public static IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            RegisterDependencies();
            ListenToMessages();

            Console.ReadLine();
        }

        private static void RegisterDependencies()
        {
            var dispatcherConfiguration = new DispatcherConfiguration();

            _serviceProvider = new ServiceCollection()

                .AddOptions()
                .Configure<RabbitMqConnectionSettings>(options => Configuration.GetSection("RabbitMQ").Bind(options))
                .Configure<MongoDbConnectionSettings>(options => Configuration.GetSection("MongoDB").Bind(options))

                .AddTransient<IMessageSerializer, DefaultMessageSerializer>()

                .AddSingleton<IChannelFactory, ChannelFactory>()
                .AddSingleton<IReadModelChannel>(sp => sp.GetService<IChannelFactory>().CreateChannel())
                .AddSingleton<ISuccessChannel>(sp => sp.GetService<IChannelFactory>().CreateChannel())

                .AddTransient<IDispatcher, EventDispatcher>()

                .AddSingleton<ShipmentEventsHandler, ShipmentEventsHandler>()

                .AddSingleton<DispatcherConfiguration>(dispatcherConfiguration)
                .AddSingleton<ILogger>(Log.Logger)

                .AddSingleton<IMongoClient>(sp => new MongoClient(sp.GetService<IOptions<MongoDbConnectionSettings>>().Value.ConnectionString))
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
