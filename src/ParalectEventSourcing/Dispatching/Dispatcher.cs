// <copyright file="Dispatcher.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Dispatching
{
    using Exceptions;
    using Microsoft.Practices.ServiceLocation;
    using Serilog;

    /// <summary>
    /// Dispatcher
    /// </summary>
    public class Dispatcher : IDispatcher
    {
        private readonly IDispatcherHandlerRegistry _dispatcherHandlerRegistry;

        private ILogger _logger = Log.Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dispatcher"/> class.
        /// </summary>
        /// <param name="dispatcherHandlerRegistry">registry</param>
        /// <param name="serviceLocator">service locator</param>
        public Dispatcher(IDispatcherHandlerRegistry dispatcherHandlerRegistry)
        {
            _dispatcherHandlerRegistry = dispatcherHandlerRegistry;
        }

        /// <inheritdoc/>
        public void Dispatch(object message)
        {
            var subscriptions = _dispatcherHandlerRegistry.GetSubscriptions(message.GetType());

            //foreach (var subscription in subscriptions)
            //{
            //    var handler = this.serviceLocator.GetInstance(subscription.HandlerType);
            //    try
            //    {
            //        var methodInfo = subscription.GetMethodInfo();
            //        methodInfo.Invoke(handler, new[] { message });
            //    }
            //    catch (HandlerException handlerException)
            //    {
            //        this.logger.Error("Message handling failed.", handlerException);
            //    }
            //}
        }
    }
}


//// <copyright file="Dispatcher.cs" company="Advanced Metering Services LLC">
////     Copyright (c) Advanced Metering Services LLC. All rights reserved.
//// </copyright>

//namespace ParalectEventSourcing.Dispatching
//{
//    using Exceptions;
//    using Microsoft.Practices.ServiceLocation;
//    using Serilog;

//    /// <summary>
//    /// Dispatcher
//    /// </summary>
//    public class Dispatcher : IDispatcher
//    {
//        private readonly IDispatcherHandlerRegistry dispatcherHandlerRegistry;
//        private readonly IServiceLocator serviceLocator;

//        private Serilog.ILogger logger = Log.Logger;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="Dispatcher"/> class.
//        /// </summary>
//        /// <param name="dispatcherHandlerRegistry">registry</param>
//        /// <param name="serviceLocator">service locator</param>
//        public Dispatcher(IDispatcherHandlerRegistry dispatcherHandlerRegistry, IServiceLocator serviceLocator)
//        {
//            this.dispatcherHandlerRegistry = dispatcherHandlerRegistry;
//            this.serviceLocator = serviceLocator;
//        }

//        /// <inheritdoc/>
//        public void Dispatch(object message)
//        {
//            var subscriptions = this.dispatcherHandlerRegistry.GetSubscriptions(message.GetType());

//            foreach (var subscription in subscriptions)
//            {
//                var handler = this.serviceLocator.GetInstance(subscription.HandlerType);
//                try
//                {
//                    var methodInfo = subscription.GetMethodInfo();
//                    methodInfo.Invoke(handler, new[] { message });
//                }
//                catch (HandlerException handlerException)
//                {
//                    this.logger.Error("Message handling failed.", handlerException);
//                }
//            }
//        }
//    }
//}