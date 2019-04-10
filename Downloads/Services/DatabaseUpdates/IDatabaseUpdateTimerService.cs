namespace Downloads.Services.DatabaseUpdates
{
    using System;
    using System.Timers;

    public interface IDatabaseUpdateTimerService : IDisposable
    {
        event ElapsedEventHandler Elapsed;

        void Start();
    }
}