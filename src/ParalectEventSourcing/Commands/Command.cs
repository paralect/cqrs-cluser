// <copyright file="Command.cs" company="Advanced Metering Services LLC">
// Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Commands
{
    /// <summary>
    ///     Domain Command
    /// </summary>
    public abstract class Command : ICommand
    {
        /// <summary>
        /// Gets or sets ID of aggregate
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets Command metadata
        /// </summary>
        public ICommandMetadata Metadata { get; set; } = new CommandMetadata();
    }
}