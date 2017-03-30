// <copyright file="IDispatcher.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Dispatching
{
    /// <summary>
    /// Dispatcher
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Dispatches a message
        /// </summary>
        /// <param name="message">a message</param>
        void Dispatch(object message);
    }
}
