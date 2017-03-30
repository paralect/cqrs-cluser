// <copyright file="IHandlerMethodCache.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Dispatching
{
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;

    /// <summary>
    /// Caches method description
    /// </summary>
    public interface IHandlerMethodCache
    {
        /// <summary>
        /// Gets method infro for specific handler type and message type
        /// </summary>
        /// <param name="handlerType">handler type</param>
        /// <param name="messageType">message type</param>
        /// <returns>method info</returns>
        MethodInfo GetMethodInfo(Type handlerType, Type messageType);
    }

    /// <summary>
    /// Caches method description
    /// </summary>
    public class HandlerMethodCache : IHandlerMethodCache
    {
        private readonly ConcurrentDictionary<MethodDescriptor, MethodInfo> methodCache = new ConcurrentDictionary<MethodDescriptor, MethodInfo>();

        /// <inheritdoc/>
        public MethodInfo GetMethodInfo(Type handlerType, Type messageType)
        {
            var methodDescriptor = new MethodDescriptor(handlerType, messageType);
            MethodInfo methodInfo = null;
            if (!this.methodCache.TryGetValue(methodDescriptor, out methodInfo))
            {
                this.methodCache[methodDescriptor] = methodInfo = handlerType.GetMethod(DispatcherHandlerRegistry.HandleMethodName, new[] { messageType });
            }

            return methodInfo;
        }
    }
}
