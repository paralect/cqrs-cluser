// <copyright file="MethodDescriptor.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Dispatching
{
    using System;

    /// <summary>
    /// Struct of handler type and message type combination
    /// </summary>
    public struct MethodDescriptor
    {
        /// <summary>
        /// The handler type
        /// </summary>
        public readonly Type HandlerType;

        /// <summary>
        /// The message type
        /// </summary>
        public readonly Type MessageType;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDescriptor"/> struct.
        /// </summary>
        /// <param name="handlerType">handler type</param>
        /// <param name="messageType">message type</param>
        public MethodDescriptor(Type handlerType, Type messageType)
            : this()
        {
            HandlerType = handlerType;
            MessageType = messageType;
        }

        /// <summary>
        /// Equals to
        /// </summary>
        /// <param name="descriptor">target desciptor</param>
        /// <returns>true if equals</returns>
        public bool Equals(MethodDescriptor descriptor)
        {
            return descriptor.HandlerType == HandlerType && descriptor.MessageType == MessageType;
        }

        /// <summary>
        /// Equals to
        /// </summary>
        /// <param name="descriptor">target desciptor</param>
        /// <returns>true if equals</returns>
        public override bool Equals(object descriptor)
        {
            if (ReferenceEquals(null, descriptor))
            {
                return false;
            }

            if (descriptor.GetType() != typeof(MethodDescriptor))
            {
                return false;
            }

            return Equals((MethodDescriptor)descriptor);
        }

        /// <summary>
        /// Gets hash code
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((HandlerType != null ? HandlerType.GetHashCode() : 0) * 397)
                     ^ (MessageType != null ? MessageType.GetHashCode() : 0);
            }
        }
    }
}
