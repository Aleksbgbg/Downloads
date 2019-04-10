namespace Downloads.Services.DatabaseUpdates
{
    using System;
    using System.Timers;

    public interface ITimer : IDisposable
    {
        event ElapsedEventHandler Elapsed;

        double Interval { get; set; }

        void Start();
    }
}