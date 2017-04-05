// <copyright file="Aggregate{TState}.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing
{
    using System;
    using Exceptions;

    /// <summary>
    /// Aggregate root base class with generic state param
    /// </summary>
    /// <typeparam name="TState">State type</typeparam>
    public abstract class Aggregate<TState> : AggregateRoot
    {
        /// <summary>
        /// Gets aggregate state
        /// </summary>
        public new TState State => (TState) base.State;

        /// <summary>
        /// Checks if aggregate has been already created
        /// </summary>
        /// <param name="idSelector">selects ID in the state</param>
        /// <exception cref="AggregateHasBeenAlreadyCreatedException">Aggregate can't be crated twice</exception>
        protected void AggregateNotCreatedGuard(Func<TState, string> idSelector)
        {
            var id = idSelector(State);
            if (!string.IsNullOrEmpty(id))
            {
                throw new AggregateHasBeenAlreadyCreatedException(id);
            }
        }
    }
}
