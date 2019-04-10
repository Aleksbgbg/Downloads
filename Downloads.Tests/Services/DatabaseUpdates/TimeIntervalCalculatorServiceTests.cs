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
            TwentyFourHourTimeDifference twentyFourHourTimeDifference = TwentyFourHourTimeDifference.FromTo(18, 22);
            TimeSpan timeUntilDeadline = TimeSpan.FromHours(4);

            TimeSpan result = twentyFourHourTimeDifference.CalculateTimeUntilHourNextReached();

            Assert.Equal(timeUntilDeadline, result);
        }

        [Fact]
        public void TestCalculateTimeUntilDeadlineWhenDeadlineBeforeCurrentTime()
        {
            TwentyFourHourTimeDifference twentyFourHourTimeDifference = TwentyFourHourTimeDifference.FromTo(23, 22);
            TimeSpan timeUntilDeadline = TimeSpan.FromHours(23);

            TimeSpan result = twentyFourHourTimeDifference.CalculateTimeUntilHourNextReached();

            Assert.Equal(timeUntilDeadline, result);
        }

        private class TwentyFourHourTimeDifference
        {
            private readonly DateTime _startingTime;

            private readonly DateTime _deadline;

            private TwentyFourHourTimeDifference(int currentHour, int deadlineHour)
            {
                _startingTime = new DateTime(1, 1, 1, currentHour, 00, 00);
                _deadline = new DateTime(2020, 6, 15, deadlineHour, 00, 00);
            }

            public static TwentyFourHourTimeDifference FromTo(int currentHour, int deadlineHour)
            {
                return new TwentyFourHourTimeDifference(currentHour, deadlineHour);
            }

            public TimeSpan CalculateTimeUntilHourNextReached()
            {
                TimeIntervalCalculatorService timeIntervalCalculatorService = new TimeIntervalCalculatorService
                {
                    SourceTime = _startingTime
                };

                TimeSpan result = timeIntervalCalculatorService.CalculateTimeUntilDeadline(_deadline);

                return result;
            }
        }
    }
}