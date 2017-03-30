// <copyright file="Subscription.cs" company="Advanced Metering Services LLC">
// Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Dispatching
{
    using System;
    using System.Reflection;

    public class Subscription
    {
        private readonly IHandlerMethodCache handlerMethodCache;
        private readonly Type messageType;

        public Subscription(Type handlerType,
            Type messageType,
            IHandlerMethodCache handlerMethodCache,
            int priority)
        {
            this.messageType = messageType;
            this.handlerMethodCache = handlerMethodCache;
            this.HandlerType = handlerType;
            this.Priority = priority;
        }

        public Type HandlerType { get; private set; }

        public int Priority { get; private set; }

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

            return other.HandlerType == this.HandlerType && other.Priority == this.Priority;
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

            return this.Equals(
                (Subscription)obj);
        }

        public MethodInfo GetMethodInfo()
        {
            return this.handlerMethodCache.GetMethodInfo(
                this.HandlerType,
                this.messageType);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.HandlerType != null ? this.HandlerType.GetHashCode() : 0) * 397) ^ this.Priority;
            }
        }
    }
}