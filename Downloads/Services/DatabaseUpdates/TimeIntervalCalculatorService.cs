namespace Downloads.Services.DatabaseUpdates
{
    using System;

    public class TimeIntervalCalculatorService : ITimeIntervalCalculatorService
    {
        public DateTime SourceTime { get; set; }

        public TimeSpan CalculateTimeUntilDeadline(DateTime deadline)
        {
            deadline = CoerceDeadlineFollowingSourceTime(deadline);
            return deadline - SourceTime;
        }

        private DateTime CoerceDeadlineFollowingSourceTime(DateTime deadline)
        {
            TimeSpan sourceTimeDifference = SourceTime - deadline;
            double totalDayDifference = sourceTimeDifference.TotalDays;
            double differenceInFullDays = Math.Ceiling(totalDayDifference);

            return deadline.AddDays(differenceInFullDays);
        }
    }
}