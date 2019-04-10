namespace Downloads.Services.DatabaseUpdates
{
    using System.Timers;

    public interface ITimer
    {
        event ElapsedEventHandler Elapsed;

        double Interval { get; set; }

        void Start();
    }
}