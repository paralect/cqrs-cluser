// <copyright file="ICommand.cs" company="Advanced Metering Services LLC">
// Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Commands
{
    /// <summary>
    ///     Domain Command interface
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Gets or sets command metadata
        /// </summary>
        ICommandMetadata Metadata { get; set; }
    }
}