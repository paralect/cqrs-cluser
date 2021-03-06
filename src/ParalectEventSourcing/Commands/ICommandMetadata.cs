﻿// <copyright file="ICommandMetadata.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Commands
{
    using System;

    /// <summary>
    /// Command metadata interface
    /// </summary>
    public interface ICommandMetadata
    {
        /// <summary>
        /// Gets or sets unique Id of Command
        /// </summary>
        string CommandId { get; set; }

        /// <summary>
        /// Gets or sets user Id of user who initiate this command
        /// </summary>
        string UserId { get; set; }

        /// <summary>
        /// Gets or sets assembly qualified CLR Type name of Command Type
        /// </summary>
        string TypeName { get; set; }

        /// <summary>
        /// Gets or sets time when command was created
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets SignalR Connection Id
        /// </summary>
        string ConnectionId { get; set; }
    }
}
