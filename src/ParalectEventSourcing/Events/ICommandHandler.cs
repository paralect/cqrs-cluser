// <copyright file="IEventMetadata.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Events
{
    /// <summary>
    /// Command handler marker
    /// </summary>
    public interface ICommandHandler : IMessageHandler
    {
    }

    /// <summary>
    /// Command handler marker
    /// </summary>
    /// <typeparam name="T">the command type</typeparam>
    public interface ICommandHandler<T> : IMessageHandler<T>
    {
    }
}
