// <copyright file="InMemoryCommandBus.cs" company="Advanced Metering Services LLC">
// Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.InMemory
{
    using System;
    using Commands;
    using Dispatching;
    using Serilog;
    using Utils;

    /// <summary>
    /// Messages bus for commands
    /// </summary>
    public class InMemoryCommandBus : CommandBus
    {
        private static readonly ILogger Log = Serilog.Log.Logger;

        private readonly IDispatcher _dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCommandBus"/> class.
        /// </summary>
        /// <param name="dispatcher">the dispatcher</param>
        /// <param name="dateTimeProvider">datetime provider</param>
        public InMemoryCommandBus(IDispatcher dispatcher, IDateTimeProvider dateTimeProvider)
            : base(dateTimeProvider)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Send single or several messages
        /// </summary>
        /// <param name="commands">The commands.</param>
        protected override void SendInternal(params ICommand[] commands)
        {
            try
            {
                foreach (var command in commands)
                {
                    _dispatcher.Dispatch(
                        command);
                }
            }
            catch (Exception ex)
            {
                // we are not throwing exception here, because dispatching
                // may be performed asynchronously and on another machine
                // (but right now we dispatching synchronously)
                // so we can just log error message
                Log.Error(ex, "Command bus exception.");
            }
        }
    }
}