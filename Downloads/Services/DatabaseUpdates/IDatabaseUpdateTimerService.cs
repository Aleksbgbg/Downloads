namespace Downloads.Services
{
    using System;
    using System.Timers;

    public interface IDatabaseUpdateTimerService
    {
        event ElapsedEventHandler Elapsed;

        void Start();
    }
}