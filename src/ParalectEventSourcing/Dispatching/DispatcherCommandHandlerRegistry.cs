namespace ParalectEventSourcing.Dispatching
{
    using System;
    using Events;

    public class DispatcherCommandHandlerRegistry : DispatcherHandlerRegistry
    {
        public override Type MarkerInterface => typeof(ICommandHandler);
        public override Type MarkerInterfaceGeneric => typeof(ICommandHandler<>);

        public DispatcherCommandHandlerRegistry(IServiceProvider serviceLocator)
        {
            ServiceLocator = serviceLocator;
        }
    }
}
