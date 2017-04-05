// <copyright file="StateSpooler.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Utils
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Reflection;
    using Dispatching;
    using Events;

    /// <summary>
    /// State spooler utility
    /// </summary>
    public class StateSpooler
    {
        private static readonly ConcurrentDictionary<MethodDescriptor, MethodInfo> MethodCache = new ConcurrentDictionary<MethodDescriptor, MethodInfo>();

        /// <summary>
        /// Applies event on the state
        /// </summary>
        /// <param name="state">the state</param>
        /// <param name="evnt">the event</param>
        /// <exception cref="ArgumentNullException">state is null</exception>
        public static void Spool(object state, IEvent evnt)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            InvokeMethodOn(state, evnt);
        }

        /// <summary>
        /// Applies event on the state
        /// </summary>
        /// <param name="state">the state</param>
        /// <param name="events">the event</param>
        /// <exception cref="ArgumentNullException">state is null</exception>
        public static void Spool(object state, IEnumerable<IEvent> events)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            foreach (var evnt in events)
            {
                InvokeMethodOn(state, evnt);
            }
        }

        private static void InvokeMethodOn(object state, object message)
        {
            var methodDescriptor = new MethodDescriptor(state.GetType(), message.GetType());
            MethodInfo methodInfo = null;
            if (!MethodCache.TryGetValue(methodDescriptor, out methodInfo))
            {
                //MethodCache[methodDescriptor] = methodInfo = state.GetType()
                //    .GetMethod("On", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { message.GetType() }, null); TODO make it work
                MethodCache[methodDescriptor] = methodInfo = state.GetType().GetMethod("On", new [] { message.GetType() });
            }

            methodInfo?.Invoke(state, new[] { message });
        }
    }
}
