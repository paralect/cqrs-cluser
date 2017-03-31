namespace ParalectEventSourcing.Utils
{
    using System;

    public interface IDateTimeProvider
    {
        DateTime GetUtcNow();
    }
}