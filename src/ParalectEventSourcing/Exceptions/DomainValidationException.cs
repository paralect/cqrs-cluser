// <copyright file="DomainValidationException.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Exceptions
{
    using System;

    /// <summary>
    /// Exception occurs when any validation fails during command processing in the aggreagte
    /// </summary>
    public class DomainValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainValidationException"/> class.
        /// </summary>
        /// <param name="message">Validation message</param>
        public DomainValidationException(string message)
            : base(message)
        {
        }
    }
}
