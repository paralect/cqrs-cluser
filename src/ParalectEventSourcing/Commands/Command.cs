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
        ///     Command metadata
        /// </summary>
        private ICommandMetadata metadata = new CommandMetadata();

        /// <summary>
        /// Gets or sets ID of aggregate
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets Command metadata
        /// </summary>
        public ICommandMetadata Metadata
        {
            get { return this.metadata; }
            set { this.metadata = value; }
        }
    }
}