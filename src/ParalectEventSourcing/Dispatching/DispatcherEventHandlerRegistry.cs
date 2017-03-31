namespace ParalectEventSourcing.Dispatching
{
    using Events;
    using System;

    public class DispatcherEventHandlerRegistry : DispatcherHandlerRegistry
    {
        public override Type MarkerInterface => typeof(IEventHandler);
        public override Type MarkerInterfaceGeneric => typeof(IEventHandler<>);

        public DispatcherEventHandlerRegistry(IServiceProvider serviceLocator)
        {
            ServiceLocator = serviceLocator;
        }
    }
}
