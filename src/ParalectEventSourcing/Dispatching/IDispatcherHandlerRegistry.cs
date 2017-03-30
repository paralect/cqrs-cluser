// <copyright file="IDispatcherHandlerRegistry.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Dispatching
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Registry of subcriptions to use in the dispatcher
    /// </summary>
    public interface IDispatcherHandlerRegistry
    {
        /// <summary>
        /// Gets subscriptions for specific message type
        /// </summary>
        /// <param name="messageType">the message type</param>
        /// <returns>list of subscriptions</returns>
        List<Subscription> GetSubscriptions(Type messageType);

        /// <summary>
        /// Registers specifc type
        /// </summary>
        /// <param name="type">register the type</param>
        void Register(Type type);

        /// <summary>
        ///  Register all handlers in types
        /// </summary>
        /// <param name="types">the types to register</param>
        void Register(IEnumerable<Type> types);

        /// <summary>
        /// Register all handlers in assembly (you can register handlers that optionally belongs to specified namespaces)
        /// </summary>
        /// <param name="assembly">assembly to scan</param>
        /// <param name="namespaces">namespaces to scan</param>
        void Register(Assembly assembly, string[] namespaces);

        /// <summary>
        /// Register all handlers in assemblies (you can register handlers that optionally belongs to specified namespaces)
        /// </summary>
        /// <param name="assemblies">assemblies to scan</param>
        /// <param name="namespaces">namespaces to scan</param>
        void Register(Assembly[] assemblies, string[] namespaces);
    }
}
