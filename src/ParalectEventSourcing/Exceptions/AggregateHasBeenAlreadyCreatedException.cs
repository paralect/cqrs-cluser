// <copyright file="AggregateHasBeenAlreadyCreatedException.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Exceptions
{
    /// <summary>
    /// Exception occurs when an aggregate attemps to create the stream second time
    /// </summary>
    public class AggregateHasBeenAlreadyCreatedException : DomainValidationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateHasBeenAlreadyCreatedException"/> class.
        /// Creates exception
        /// </summary>
        /// <param name="id">ID of the tarhet aggregate</param>
        public AggregateHasBeenAlreadyCreatedException(string id)
            : base(string.Format("Aggregate has been already created."))
        {
            Id = id;
        }

        /// <summary>
        /// Gets ID of the target aggreate
        /// </summary>
        public string Id { get; private set; }
    }
}
