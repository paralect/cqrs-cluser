// <copyright file="HandlerException.cs" company="Advanced Metering Services LLC">
// Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Exceptions
{
    using System;

    /// <summary>
    /// Exception occurs when handler is not able to process the message
    /// </summary>
    public class HandlerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="messageObject">The message object.</param>
        public HandlerException(string message, Exception innerException, object messageObject)
            : base(message, innerException)
        {
            MessageObject = messageObject;
        }

        private object MessageObject { get; }
    }
}