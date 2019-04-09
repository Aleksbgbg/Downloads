namespace Downloads.Services.DatabaseUpdates
{
    using System;

    public class TimeIntervalCalculatorService : ITimeIntervalCalculatorService
    {
        public DateTime SourceTime { get; set; }

        public TimeSpan CalculateTimeUntilDeadline(DateTime deadline)
        {
            return deadline - SourceTime;
        }
    }
}