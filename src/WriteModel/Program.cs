namespace WriteModel
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using CommandHandlers;
    using Domain;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
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
    using System.IO;
    using ParalectEventSourcing.Persistence.Redis;

    public class Program
    {
        private static IServiceProvider _serviceProvider;
        private static string _environment;

        public static IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] args)
        {
            _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{_environment}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "redis";
                options.InstanceName = "EsSnapshots";
            });

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
                .Configure<EventStoreConnectionSettings>(options => Configuration.GetSection("EventStore").Bind(options));
        }

        private static void RegisterCommonServices(IServiceCollection services)
        {
            var dispatcherConfiguration = new DispatcherConfiguration();

            if (_environment == "Development")
            {
                services.AddTransient<IEventSource, HostEventSource>();
            }
            else
            {
                services.AddTransient<IEventSource, ClusterEventSource>();
            }

            _serviceProvider = services
              
                .AddSingleton<IChannelFactory, ChannelFactory>()
                .AddSingleton<IWriteModelChannel>(sp => sp.GetService<IChannelFactory>().CreateChannel())
                .AddSingleton<IReadModelChannel>(sp => sp.GetService<IChannelFactory>().CreateChannel())
                .AddSingleton<IErrorChannel>(sp => sp.GetService<IChannelFactory>().CreateChannel())

                .AddTransient<IMessageSerializer, DefaultMessageSerializer>()
                .AddTransient<IEventBus, RabbitMqEventBus>()
                .AddSingleton<IDispatcher, CommandDispatcher>()
                .AddTransient<IDateTimeProvider, DateTimeProvider>()
                .AddTransient<IEventStoreSerializer, MessagePackEventStoreSerializer>()
                .AddTransient<ISnapshotRepository, RedisSnapshotRepository>()
                .AddSingleton<DispatcherConfiguration>(dispatcherConfiguration)
                .AddSingleton<ILogger>(Log.Logger)

                .AddSingleton<ShipmentCommandsHandler, ShipmentCommandsHandler>()
                .AddTransient<IAggregateRepository<Shipment>, AggregateRepository<Shipment>>()

                .BuildServiceProvider();

            dispatcherConfiguration.ServiceLocator = _serviceProvider;

            dispatcherConfiguration
                .DispatcherCommandHandlerRegistry
                .Register(Assembly.GetEntryAssembly(), new[] { typeof(ShipmentCommandsHandler).Namespace });
        }

        private static void ListenToMessages()
        {
            Task.Run(() =>
            {
                var channel = _serviceProvider.GetService<IWriteModelChannel>();
                channel.SubscribeToQueue(RabbitMqRoutingConfiguration.WriteModelQueue, ConsumerOnReceived);
            });
        }

        private static void ConsumerOnReceived(object sender, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            var messageSerializer = _serviceProvider.GetService<IMessageSerializer>();
            var command = messageSerializer.Deserialize(basicDeliverEventArgs.Body, c => c.Metadata.TypeName);

            try
            {
                var commandDispatcher = _serviceProvider.GetService<IDispatcher>();
                commandDispatcher.Dispatch(command);

                Console.WriteLine($"Command {command.Metadata.CommandId} handled successfully.");
            }
            catch (DomainValidationException e)
            {
                var channel = _serviceProvider.GetService<IErrorChannel>();
                channel.SendToExchange(
                    RabbitMqRoutingConfiguration.ErrorExchange,
                    (string) command.Metadata.ConnectionId,
                    new
                    {
                        OriginalCommand = command,
                        ErrorMessage = e.Message
                    });
            }
        }
    }
}
