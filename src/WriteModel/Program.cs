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
            ListenToCommands();
        }

        private static void RegisterDependencies()
        {
            var dispatcherConfiguration = new DispatcherConfiguration();
            dispatcherConfiguration
                .DispatcherHandlerRegistry
                .Register(typeof(Program).GetTypeInfo().Assembly, new[] { "WriteModel.CommandHandlers", "WriteModel.EventHandlers" });

            _serviceProvider = new ServiceCollection()
                .AddTransient<ICommandBus, CommandBus>()
                .AddTransient<IDateTimeProvider, DateTimeProvider>()
                .AddTransient<DeviceCommandsHandler, DeviceCommandsHandler>()
                .AddTransient<DeviceEventsHandler, DeviceEventsHandler>()
                .AddTransient<IAggregateRepository<Device>, AggregateRepository<Device>>()
                .AddTransient<IEventSource, InMemoryEventSource>()
                .AddTransient<IEventBus, DispatcherEventBus>()
                .AddTransient<IDispatcher, CommandDispatcher>()
                .AddTransient<ISnapshotRepository, InMemorySnapshotRepository>()
                .AddSingleton(dispatcherConfiguration)
                .AddSingleton(Log.Logger)
                .BuildServiceProvider();

            dispatcherConfiguration.ServiceLocator = _serviceProvider;
        }

        private static void ListenToCommands()
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
