// <copyright file="CommandBus.cs" company="Advanced Metering Services LLC">
// Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Commands
{
    using System;
    using Dispatching;
    using Serilog;

    /// <summary>
    ///     Messages bus for commands
    /// </summary>
    public class CommandBus : ICommandBus
    {
        private static readonly ILogger Log = Serilog.Log.Logger;

        private readonly IDispatcher dispatcher;

        private readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBus"/> class.
        /// </summary>
        /// <param name="dispatcher">the dispatcher</param>
        /// <param name="dateTimeProvider">datetime provider</param>
        public CommandBus(IDispatcher dispatcher, IDateTimeProvider dateTimeProvider)
        {
            this.dispatcher = dispatcher;
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// Send single or several messages
        /// </summary>
        /// <param name="commands">The commands.</param>
        public void Send(params ICommand[] commands)
        {
            this.PrepareCommands(commands);

            try
            {
                foreach (var command in commands)
                {
                    this.dispatcher.Dispatch(
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

        /// <summary>
        /// Prepare commands before they reach adressee
        /// </summary>
        /// <param name="commands">The commands.</param>
        private void PrepareCommands(
            params ICommand[] commands)
        {
            foreach (ICommand command in commands)
            {
                command.Metadata.CommandId = Guid.NewGuid().ToString();
                command.Metadata.CreatedDate = this.dateTimeProvider.GetUtcNow();
                command.Metadata.TypeName = command.GetType().FullName;
            }
        }
    }

    public interface IDateTimeProvider
    {
        DateTime GetUtcNow();
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}