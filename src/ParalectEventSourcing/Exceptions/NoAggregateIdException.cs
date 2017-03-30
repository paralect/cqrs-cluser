// <copyright file="NoAggregateIdException.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Exceptions
{
    using System;

    /// <summary>
    /// No agggregate ID specified
    /// </summary>
    public class NoAggregateIdException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoAggregateIdException"/> class.
        /// </summary>
        /// <param name="message">The message</param>
        public NoAggregateIdException(string message)
            : base(message)
        {
        }
    }
}
