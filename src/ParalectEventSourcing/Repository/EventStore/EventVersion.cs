// <copyright file="EventVersion.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Repository.EventStore
{
    /// <summary>
    /// Version of event to be expected when saving to the event store
    /// </summary>
    public enum EventVersion
    {
        /// <summary>
        /// Stream is not created
        /// </summary>
        NoStream = -1,

        /// <summary>
        /// Any version excpected
        /// </summary>
        Any = -2
    }
}
