// <copyright file="DispatcherConfiguration.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Dispatching
{
    using System;

    /// <summary>
    /// Dispatcher configuration
    /// </summary>
    public class DispatcherConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DispatcherConfiguration"/> class.
        /// </summary>
        public DispatcherConfiguration()
        {
            this.DispatcherHandlerRegistry = new DispatcherHandlerRegistry();
            this.NumberOfRetries = 1;
        }

        /// <summary>
        /// Gets or sets handlers registry
        /// </summary>
        public DispatcherHandlerRegistry DispatcherHandlerRegistry { get; set; }

        /// <summary>
        /// Gets or sets number of retries
        /// </summary>
        public int NumberOfRetries { get; set; }

        /// <summary>
        /// Gets or sets service locator
        /// </summary>
        public IServiceProvider ServiceLocator { get; set; }

        /// <summary>
        /// Gets or sets message Handler Marker interface
        /// </summary>
        public Type MessageHandlerMarkerInterface { get; set; }
    }
}