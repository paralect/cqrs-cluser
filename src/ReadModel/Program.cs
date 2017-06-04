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
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            var services = new ServiceCollection();
            RegisterConnectionSettings(services);
            RegisterCommonServices(services);

            ListenToMessages();

            Console.ReadLine();
        }

        private static void RegisterConnectionSettings(IServiceCollection services)
        {
            services
                .AddOptions()
                .Configure<RabbitMqConnectionSettings>(options => Configuration.GetSection("RabbitMQ").Bind(options))
                .Configure<MongoDbConnectionSettings>(options => Configuration.GetSection("MongoDB").Bind(options));
        }

        private static void RegisterCommonServices(IServiceCollection services)
        {
            var dispatcherConfiguration = new DispatcherConfiguration();

            _serviceProvider = services

                .AddSingleton<IChannelFactory, ChannelFactory>()
                .AddSingleton<IReadModelChannel>(sp => sp.GetService<IChannelFactory>().CreateChannel())
                .AddSingleton<ISuccessChannel>(sp => sp.GetService<IChannelFactory>().CreateChannel())

                .AddTransient<ISerializer, DefaultSerializer>()
                .AddTransient<IDispatcher, EventDispatcher>()
                .AddSingleton<DispatcherConfiguration>(dispatcherConfiguration)
                .AddSingleton<ILogger>(Log.Logger)

                .AddSingleton<ShipmentEventsHandler, ShipmentEventsHandler>()

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

        private static void ConsumerOnReceived(object sender, BasicDeliverEventArgs e)
        {
            var messageSerializer = _serviceProvider.GetService<ISerializer>();
            var @event = messageSerializer.Deserialize(e.Body, evt => evt.Metadata.TypeName);

            var eventDispatcher = _serviceProvider.GetService<IDispatcher>();
            eventDispatcher.Dispatch(@event);

            Console.WriteLine($"Event {@event.Metadata.EventId} handled successfully.");

            var readModelChannel = _serviceProvider.GetService<IReadModelChannel>();
            readModelChannel.Ack(e.DeliveryTag);
        }
    }
}
