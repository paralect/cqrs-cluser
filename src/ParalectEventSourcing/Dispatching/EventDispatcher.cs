namespace ParalectEventSourcing.Dispatching
{
    using System;
    using Exceptions;
    using Serilog;

    public class EventDispatcher : IEventDispatcher
    {
        private readonly ILogger _log;

        /// <summary>
        /// Registry of all registered handlers
        /// </summary>
        private readonly DispatcherEventHandlerRegistry _registry;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventDispatcher"/> class.
        /// </summary>
        /// <param name="configuration">dispatcher configuration</param>
        /// <param name="logger">logger</param>
        public EventDispatcher(DispatcherConfiguration configuration, ILogger logger)
        {
            _log = logger;
            if (configuration.ServiceLocator == null)
            {
                throw new ArgumentException("Unity Container is not registered for distributor.");
            }

            if (configuration.DispatcherEventHandlerRegistry == null)
            {
                throw new ArgumentException("Dispatcher Event Handler Registry is null in distributor.");
            }

            _registry = configuration.DispatcherEventHandlerRegistry;

            // order handlers
            _registry.InsureOrderOfHandlers();
        }

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
            catch (Exception exception)
            {
                _log.Error(exception, "{0}", new[] { "Error when dispatching message" });
            }
        }

        private void ExecuteHandler(object handler, object message, Action<Exception> exceptionObserver = null)
        {
            try
            {
                InvokeDynamic(handler, message);
            }
            catch (Exception exception)
            {
                _log.Error(exception, "Exception in the handler {0} for message {1}", handler.GetType().FullName, message.GetType().FullName);
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
