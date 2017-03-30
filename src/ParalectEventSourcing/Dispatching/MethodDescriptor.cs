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
            this.HandlerType = handlerType;
            this.MessageType = messageType;
        }

        /// <summary>
        /// Equals to
        /// </summary>
        /// <param name="descriptor">target desciptor</param>
        /// <returns>true if equals</returns>
        public bool Equals(MethodDescriptor descriptor)
        {
            return descriptor.HandlerType == this.HandlerType && descriptor.MessageType == this.MessageType;
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

            return this.Equals((MethodDescriptor)descriptor);
        }

        /// <summary>
        /// Gets hash code
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.HandlerType != null ? this.HandlerType.GetHashCode() : 0) * 397)
                     ^ (this.MessageType != null ? this.MessageType.GetHashCode() : 0);
            }
        }
    }
}
