namespace Downloads.Services.DatabaseUpdates
{
    using System;
    using System.Timers;

    using Downloads.Infrastructure.Options;

    using Microsoft.Extensions.Options;

    public class DatabaseUpdateTimerService : IDatabaseUpdateTimerService
    {
        private readonly ITimer _timer;

        private readonly ITimeIntervalCalculatorService _timeIntervalCalculatorService;

        private readonly IOptions<DatabaseTimerOptions> _databaseTimerOptions;

        private DateTime _updateTime;

        public DatabaseUpdateTimerService(ITimer timer, ITimeIntervalCalculatorService timeIntervalCalculatorService, IOptions<DatabaseTimerOptions> databaseTimerOptions)
        {
            _timer = timer;
            _timeIntervalCalculatorService = timeIntervalCalculatorService;
            _databaseTimerOptions = databaseTimerOptions;
        }

        public event ElapsedEventHandler Elapsed;

        public void Start()
        {
            _updateTime = _databaseTimerOptions.Value.UpdateTime;

            CalculateTimerInterval();
            _timer.Elapsed += TimerElapsedHandler;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            _timer.Elapsed -= TimerElapsedHandler;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        private void TimerElapsedHandler(object sender, ElapsedEventArgs e)
        {
            _updateTime = _updateTime.AddDays(1.0);
            CalculateTimerInterval();

            Elapsed?.Invoke(sender, e);
        }

        private void CalculateTimerInterval()
        {
            _timer.Interval = _timeIntervalCalculatorService.CalculateTimeUntilDeadline(_updateTime).TotalMilliseconds;
        }
    }
}