namespace Downloads.Tests.Services.DatabaseUpdates
{
    using System;

    using Downloads.Services.DatabaseUpdates;

    using Xunit;

    public class TimeIntervalCalculatorServiceTests
    {
        [Fact]
        public void TestCalculateTimeUntilDeadline()
        {
            DateTime now = new DateTime(2020, 6, 15, 18, 00, 00);
            DateTime deadline = new DateTime(now.Year, now.Month, now.Day, 22, 00, 00);
            TimeSpan timeUntilDeadline = deadline - now;

            TimeIntervalCalculatorService timeIntervalCalculatorService = new TimeIntervalCalculatorService
            {
                SourceTime = now
            };

            TimeSpan result = timeIntervalCalculatorService.CalculateTimeUntilDeadline(deadline);

            Assert.Equal(timeUntilDeadline, result);
        }
    }
}