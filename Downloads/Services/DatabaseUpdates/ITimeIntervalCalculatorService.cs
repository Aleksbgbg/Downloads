namespace Downloads.Services.DatabaseUpdates
{
    using System;

    public interface ITimeIntervalCalculatorService
    {
        DateTime SourceTime { get; set; }

        TimeSpan CalculateTimeUntilDeadline(DateTime deadline);
    }
}