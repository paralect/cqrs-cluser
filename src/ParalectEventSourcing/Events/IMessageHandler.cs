// <copyright file="IMessageHandler.cs" company="Advanced Metering Services LLC">
// Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>
namespace ParalectEventSourcing.Events
{
    /// <summary>
    /// Message handler marker
    /// </summary>
    public interface IMessageHandler
    {
    }

    /// <summary>
    /// Message handler marker
    /// </summary>
    /// <typeparam name="T">the message type</typeparam>
    public interface IMessageHandler<T>
    {
    }
}