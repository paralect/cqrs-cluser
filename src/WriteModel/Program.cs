namespace WriteModel
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using CommandHandlers;
    using Domain;
    using EventHandlers;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Practices.ServiceLocation;
    using Newtonsoft.Json;
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
            var dispatcherConfiguration = new DispatcherConfiguration();
            dispatcherConfiguration
                .DispatcherHandlerRegistry
                .Register(typeof(Program).GetTypeInfo().Assembly, new [] { "WriteModel.CommandHandlers", "WriteModel.EventHandlers" });

            _serviceProvider = new ServiceCollection()
                .AddTransient<ICommandHandler, DeviceCommandsHandler>()
                .AddTransient<IAggregateRepository<Device>, AggregateRepository<Device>>()
                .AddTransient<IEventSource, InMemoryEventSource>()
                .AddTransient<IEventBus, DispatcherEventBus>()
                .AddTransient<IDispatcher, CommandDispatcher>()
                .AddTransient<ISnapshotRepository, InMemorySnapshotRepository>()
                .AddSingleton(new DeviceEventsHandler())
                .AddSingleton(dispatcherConfiguration)
                .AddSingleton(Log.Logger)
                .BuildServiceProvider();

            dispatcherConfiguration.ServiceLocator = new ServiceLocator(_serviceProvider);

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
            var typeName = ((dynamic)message).Metadata.TypeName.ToString();
            var messageType = Type.GetType(typeName);

            var typedMessage = JsonConvert.DeserializeObject(body, messageType);

            Console.WriteLine(" [x] Received {0}", message);

            dynamic dynamicHandler = _serviceProvider.GetService<ICommandHandler>();
            dynamic dynamicMessage = typedMessage;

            dynamicHandler.Handle(dynamicMessage);
        }
    }

    public class ServiceLocator : IServiceLocator
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceLocator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public object GetInstance(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        public object GetInstance(Type serviceType, string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public TService GetInstance<TService>()
        {
            throw new NotImplementedException();
        }

        public TService GetInstance<TService>(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            throw new NotImplementedException();
        }
    }
}
