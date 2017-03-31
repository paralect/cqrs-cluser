// <copyright file="Subscription.cs" company="Advanced Metering Services LLC">
// Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Dispatching
{
    using System;
    using System.Reflection;
    using Events;

    public class Subscription
    {
        private readonly IHandlerMethodCache _handlerMethodCache;
        private readonly Type _messageType;

        public Subscription(
            Type messageType,
            IHandlerMethodCache handlerMethodCache,
            int priority,
            IMessageHandler handler)
        {
            _messageType = messageType;
            _handlerMethodCache = handlerMethodCache;
            Priority = priority;
            Handler = handler;
        }

        public IMessageHandler Handler { get; }

        public Type HandlerType => Handler.GetType();

        public int Priority { get; }

        public bool Equals(
            Subscription other)
        {
            if (ReferenceEquals(
                null,
                other))
            {
                return false;
            }

            if (ReferenceEquals(
                this,
                other))
            {
                return true;
            }

            return other.HandlerType == HandlerType && other.Priority == Priority;
        }

        public override bool Equals(
            object obj)
        {
            if (ReferenceEquals(
                null,
                obj))
            {
                return false;
            }

            if (ReferenceEquals(
                this,
                obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(Subscription))
            {
                return false;
            }

            return Equals(
                (Subscription)obj);
        }

        public MethodInfo GetMethodInfo()
        {
            return _handlerMethodCache.GetMethodInfo(
                HandlerType,
                _messageType);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((HandlerType?.GetHashCode() ?? 0) * 397) ^ Priority;
            }
        }
    }
}