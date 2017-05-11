namespace WriteModel
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using CommandHandlers;
    using Domain;
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
                .AddSingleton<IWriteModelChannel>(sp => sp.GetService<IChannelFactory>().CreateChannel())
                .AddSingleton<IReadModelChannel>(sp => sp.GetService<IChannelFactory>().CreateChannel())
                .AddSingleton<IErrorChannel>(sp => sp.GetService<IChannelFactory>().CreateChannel())

                .AddTransient<IEventBus, RabbitMqEventBus>()

                .AddSingleton<IDispatcher, CommandDispatcher>()

                .AddSingleton<ShipmentCommandsHandler, ShipmentCommandsHandler>()
            
                .AddTransient<IDateTimeProvider, DateTimeProvider>()

                .AddTransient<IAggregateRepository<Shipment>, AggregateRepository<Shipment>>()

                .AddTransient<IEventSource, EventSource>()
                .AddTransient<IEventStoreSerializer, MessagePackEventStoreSerializer>()
                .AddTransient<EventStoreConnectionSettings, EventStoreConnectionSettings>()

                .AddTransient<ISnapshotRepository, InMemorySnapshotRepository>()

                .AddSingleton<DispatcherConfiguration>(dispatcherConfiguration)
                .AddSingleton<ILogger>(Log.Logger)
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
