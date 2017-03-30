// <copyright file="PriorityAttribute.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Dispatching
{
    using System;

    /// <summary>
    /// Priority defines orders of handlers execution.
    /// Handlers that have the same priority are supposed to run in parallel
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PriorityAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityAttribute"/> class.
        /// </summary>
        /// <param name="priority">priority</param>
        public PriorityAttribute(int priority)
        {
            this.Priority = priority;
        }

        /// <summary>
        /// Gets or sets priority
        /// </summary>
        public int Priority { get; set; }
    }
}