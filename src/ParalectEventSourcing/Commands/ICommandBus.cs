// <copyright file="ICommandBus.cs" company="Advanced Metering Services LLC">
// Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Commands
{
    /// <summary>
    ///     Messages bus for commands
    /// </summary>
    public interface ICommandBus
    {
        /// <summary>
        /// Sends commands to write model
        /// </summary>
        /// <param name="commands">commands</param>
        void Send(
            params ICommand[] commands);
    }
}