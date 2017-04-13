namespace ParalectEventSourcing.Commands
{
    using Utils;

    public abstract class CommandBus : ICommandBus
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        protected CommandBus(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public void Send(params ICommand[] commands)
        {
            PrepareCommands(commands);
            SendInternal(commands);
        }

        protected abstract void SendInternal(params ICommand[] commands);

        /// <summary>
        /// Prepare commands before they reach adressee
        /// </summary>
        /// <param name="commands">The commands.</param>
        private void PrepareCommands(
            params ICommand[] commands)
        {
            foreach (ICommand command in commands)
            {
                command.Metadata.CreatedDate = _dateTimeProvider.GetUtcNow();
                command.Metadata.TypeName = command.GetType().AssemblyQualifiedName;
            }
        }
    }
}
