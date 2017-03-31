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
            NumberOfRetries = 3;
        }

        /// <summary>
        /// Gets or sets command handlers registry
        /// </summary>
        public DispatcherCommandHandlerRegistry DispatcherCommandHandlerRegistry { get; set; }

        /// <summary>
        /// Gets or sets event handlers registry
        /// </summary>
        public DispatcherEventHandlerRegistry DispatcherEventHandlerRegistry { get; set; }

        /// <summary>
        /// Gets or sets number of retries
        /// </summary>
        public int NumberOfRetries { get; set; }

        private IServiceProvider _serviceLocator;

        /// <summary>
        /// Gets or sets service locator
        /// </summary>
        public IServiceProvider ServiceLocator {
            get { return _serviceLocator; }
            set
            {
                _serviceLocator = value;
                DispatcherCommandHandlerRegistry = new DispatcherCommandHandlerRegistry(_serviceLocator);
                DispatcherEventHandlerRegistry = new DispatcherEventHandlerRegistry(_serviceLocator);
            }
        }
    }
}