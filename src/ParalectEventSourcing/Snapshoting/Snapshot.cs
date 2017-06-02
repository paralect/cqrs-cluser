// <copyright file="Snapshot.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Snapshoting
{
    /// <summary>
    /// The snapshot of the aggregate state
    /// </summary>
    public class Snapshot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Snapshot"/> class.
        /// </summary>
        /// <param name="streamId">aggregate id</param>
        /// <param name="streamVersionId">aggregate version</param>
        /// <param name="payload">aggregate state</param>
        public Snapshot(string streamId, int streamVersionId, byte[] payload)
        {
            StreamId = streamId;
            StreamVersion = streamVersionId;
            Payload = payload;
        }

        /// <summary>
        /// Gets or sets stream ID
        /// </summary>
        public string StreamId { get; set; }

        /// <summary>
        /// Gets or sets stream version
        /// </summary>
        public int StreamVersion { get; set; }

        /// <summary>
        /// Gets or sets payload
        /// </summary>
        public byte[] Payload { get; set; }
    }
}
