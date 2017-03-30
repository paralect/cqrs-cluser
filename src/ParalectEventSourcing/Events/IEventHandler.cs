// <copyright file="IEventMetadata.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Events
{
    /// <summary>
    /// Event handler marker
    /// </summary>
    public interface IEventHandler : IMessageHandler
    {
    }

    /// <summary>
    /// Event handler marker
    /// </summary>
    /// <typeparam name="T">the event type</typeparam>
    public interface IEventHandler<T> : IMessageHandler<T>
    {
    }
}
