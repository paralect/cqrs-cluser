// <copyright file="DuplicateTransitionException.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Exceptions
{
    using System;

    /// <summary>
    /// Exception occures when concurency write to event store is performed
    /// </summary>
    public class DuplicateTransitionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateTransitionException"/> class.
        /// </summary>
        /// <param name="streamId">Aggreate/stream ID</param>
        /// <param name="version">Aggreagte version</param>
        /// <param name="innerException">original exception from event store</param>
        public DuplicateTransitionException(string streamId, int version, Exception innerException)
            : base(string.Format("Transition ({0}, {1}) already exists.", streamId, version), innerException)
        {
            VersionId = version;
            StreamId = streamId;
        }

        /// <summary>
        /// Gets version
        /// </summary>
        public int VersionId { get; private set; }

        /// <summary>
        /// Gets stream ID
        /// </summary>
        public string StreamId { get; private set; }
    }
}