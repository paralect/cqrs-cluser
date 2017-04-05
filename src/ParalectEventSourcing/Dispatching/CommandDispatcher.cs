// <copyright file="CommandDispatcher.cs" company="Advanced Metering Services LLC">
// Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Dispatching
{
    using System;
    using Exceptions;
    using Serilog;

    /// <summary>
    /// Dispatches a command in CQRS app
    /// </summary>
    public class CommandDispatcher : IDispatcher
    {
        private readonly ILogger _log;

        /// <summary>
        /// Registry of all registered handlers
        /// </summary>
        private readonly DispatcherCommandHandlerRegistry _registry;

        /// <summary>
        /// Number of retries in case exception was logged
        /// </summary>
        private readonly int _maxRetries;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandDispatcher"/> class.
        /// </summary>
        /// <param name="configuration">dispatcher configuration</param>
        /// <param name="logger">logger</param>
        public CommandDispatcher(DispatcherConfiguration configuration, ILogger logger)
        {
            _log = logger;
            if (configuration.ServiceLocator == null)
            {
                throw new ArgumentException("Unity Container is not registered for distributor.");
            }

            if (configuration.DispatcherCommandHandlerRegistry == null)
            {
                throw new ArgumentException("Dispatcher Command Handler Registry is null in distributor.");
            }

            _registry = configuration.DispatcherCommandHandlerRegistry;
            _maxRetries = configuration.NumberOfRetries;

            // order handlers
            _registry.InsureOrderOfHandlers();
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
            Dispatch(message, null);
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
                var subscriptions = _registry.GetSubscriptions(message.GetType());
                foreach (var subscription in subscriptions)
                {
                    try
                    {
                        ExecuteHandler(subscription.Handler, message, exceptionObserver);
                    }
                    catch (HandlerException handlerException)
                    {
                        _log.Error(handlerException, "{0}", new[] { "Message handling failed." });
                    }
                }
            }
            catch (DomainValidationException)
            {
                throw;
            }
            catch (Exception exception)
            {
                _log.Error(exception, "{0}", new[] { "Error when dispatching message" });
            }
        }

        private void ExecuteHandler(object handler, object message, Action<Exception> exceptionObserver = null)
        {
            var attempt = 0;
            while (attempt < _maxRetries)
            {
                try
                {
                    InvokeDynamic(handler, message);
                    break;
                }
                catch (DomainValidationException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    exceptionObserver?.Invoke(exception);

                    attempt++;

                    if (attempt == _maxRetries)
                    {
                        throw new HandlerException($"Exception in the handler {handler.GetType().FullName} for message {message.GetType().FullName}", exception, message);
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