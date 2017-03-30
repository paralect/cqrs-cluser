// <copyright file="CommandDispatcher.cs" company="Advanced Metering Services LLC">
// Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Dispatching
{
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;
    using Exceptions;
    using Microsoft.Practices.ServiceLocation;
    using Serilog;

    /// <summary>
    /// Dispatches a command in CQRS app
    /// </summary>
    public class CommandDispatcher : IDispatcher
    {
        private readonly ILogger log;

        /// <summary>
        /// Service Locator that is used to create handlers
        /// </summary>
        private readonly IServiceLocator serviceLocator;

        /// <summary>
        /// Registry of all registered handlers
        /// </summary>
        private readonly DispatcherHandlerRegistry registry;

        /// <summary>
        /// Number of retries in case exception was logged
        /// </summary>
        private readonly int maxRetries;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandDispatcher"/> class.
        /// </summary>
        /// <param name="configuration">dispatcher configuration</param>
        /// <param name="logger">logger</param>
        public CommandDispatcher(DispatcherConfiguration configuration, ILogger logger)
        {
            this.log = logger;
            if (configuration.ServiceLocator == null)
            {
                throw new ArgumentException("Unity Container is not registered for distributor.");
            }

            if (configuration.DispatcherHandlerRegistry == null)
            {
                throw new ArgumentException("Dispatcher Handler Registry is null in distributor.");
            }

            this.serviceLocator = configuration.ServiceLocator;
            this.registry = configuration.DispatcherHandlerRegistry;
            this.maxRetries = configuration.NumberOfRetries;

            // order handlers
            this.registry.InsureOrderOfHandlers();
        }

        /// <summary>
        /// Factory method
        /// </summary>
        /// <returns>command dispatcher instance</returns>
        /// <param name="configurationAction">configure here</param>
        /// <param name="loggingFactory">Logger</param>
        public static CommandDispatcher Create(Func<DispatcherConfiguration, DispatcherConfiguration> configurationAction, ILogger loggingFactory)
        {
            var config = new DispatcherConfiguration();
            configurationAction(config);
            return new CommandDispatcher(config, loggingFactory);
        }

        /// <summary>
        /// Dispatches a message
        /// </summary>
        /// <param name="message">message</param>
        public void Dispatch(object message)
        {
            this.Dispatch(message, null);
        }

        /// <summary>
        /// Dispatches a message with exception observer
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="exceptionObserver">exception observer</param>
        public void Dispatch(object message, Action<Exception> exceptionObserver)
        {
            try
            {
                var subscriptions = this.registry.GetSubscriptions(message.GetType());

                foreach (var subscription in subscriptions)
                {
                    var handler = this.serviceLocator.GetInstance(subscription.HandlerType);

                    try
                    {
                        this.ExecuteHandler(handler, message, exceptionObserver);
                    }
                    catch (HandlerException handlerException)
                    {
                        this.log.Error(handlerException, "{0}", new string[] { "Message handling failed." });
                    }
                }
            }
            catch (Exception exception)
            {
                this.log.Error(exception, "{0}", new string[] { "Error when dispatching message" });
            }
        }

        private void ExecuteHandler(object handler, object message, Action<Exception> exceptionObserver = null)
        {
            var attempt = 0;
            while (attempt < this.maxRetries)
            {
                try
                {
                    this.InvokeDynamic(handler, message);

                    // message handled correctly - so that should be
                    // the final attempt
                    attempt = this.maxRetries;
                }
                catch (Exception exception)
                {
                    if (exceptionObserver != null)
                    {
                        exceptionObserver(exception);
                    }

                    attempt++;

                    if (attempt == this.maxRetries)
                    {
                        throw new HandlerException(string.Format("Exception in the handler {0} for message {1}", handler.GetType().FullName, message.GetType().FullName), exception, message);
                    }
                }
            }
        }

        private void InvokeDynamic(object handler, object message)
        {
            dynamic dynamicHandler = handler;
            dynamic dynamicMessage = message;

            dynamicHandler.Handle(dynamicMessage);
        }
    }
}