namespace Downloads.Tests.Services.DatabaseUpdates
{
    using System;

    using Downloads.Infrastructure.Options;
    using Downloads.Services.DatabaseUpdates;

    using Microsoft.Extensions.Options;

    using Moq;

    using Xunit;

    public class DatabaseUpdateTimerServiceTests
    {
        private readonly Mock<ITimer> _timerMock;

        private readonly Mock<ITimeIntervalCalculatorService> _timeIntervalCalculatorMock;

        private readonly DatabaseUpdateTimerService _databaseUpdateTimerService;

        private DateTime _updateTime;

        public DatabaseUpdateTimerServiceTests()
        {
            _timerMock = new Mock<ITimer>();

            _timeIntervalCalculatorMock = new Mock<ITimeIntervalCalculatorService>();

            Mock<IOptions<DatabaseTimerOptions>> databaseTimerOptionsMock = new Mock<IOptions<DatabaseTimerOptions>>();
            databaseTimerOptionsMock.SetupGet(databaseTimerOptions => databaseTimerOptions.Value)
                                    .Returns(() => new DatabaseTimerOptions
                                    {
                                        UpdateTime = _updateTime
                                    });

            _databaseUpdateTimerService = new DatabaseUpdateTimerService(_timerMock.Object, _timeIntervalCalculatorMock.Object, databaseTimerOptionsMock.Object);
        }

        [Fact]
        public void TestStartsTimer()
        {
            _databaseUpdateTimerService.Start();
            _timerMock.Verify(timer => timer.Start(), Times.Once);
        }

        [Fact]
        public void TestSetsAppropriateInterval()
        {
            TimeSpan interval = TimeSpan.FromMilliseconds(5.0);

            _timeIntervalCalculatorMock.Setup(timeIntervalCalculator => timeIntervalCalculator.CalculateTimeUntilDeadline(_updateTime))
                                       .Returns(interval);

            _databaseUpdateTimerService.Start();

            _timeIntervalCalculatorMock.Verify(timeIntervalCalculator => timeIntervalCalculator.CalculateTimeUntilDeadline(_updateTime), Times.Once);
            _timerMock.VerifySet(timer => timer.Interval = interval.TotalMilliseconds, Times.Once);
        }

        [Fact]
        public void TestResetsIntervalAfterFirstElapsed()
        {
            TimeSpan firstTimeSpan = TimeSpan.FromHours(12);
            TimeSpan secondTimeSpan = TimeSpan.FromHours(24);

            _timeIntervalCalculatorMock.SetupSequence(timeIntervalCalculator => timeIntervalCalculator.CalculateTimeUntilDeadline(It.IsAny<DateTime>()))
                                       .Returns(firstTimeSpan)
                                       .Returns(secondTimeSpan);

            _databaseUpdateTimerService.Start();

            _timerMock.Raise(timer => timer.Elapsed += null, (EventArgs)null);

            _timerMock.VerifySet(timer => timer.Interval = firstTimeSpan.TotalMilliseconds, Times.Once);
            _timerMock.VerifySet(timer => timer.Interval = secondTimeSpan.TotalMilliseconds, Times.Once);
            _timeIntervalCalculatorMock.Verify(timeIntervalCalculator => timeIntervalCalculator.CalculateTimeUntilDeadline(_updateTime), Times.Once);
            _timeIntervalCalculatorMock.Verify(timeIntervalCalculator => timeIntervalCalculator.CalculateTimeUntilDeadline(_updateTime.AddDays(1.0)), Times.Once);
        }

        [Fact]
        public void TestInvokesElapsedEvent()
        {
            const int expectedInvocations = 3;

            int invocationCount = 0;
            _databaseUpdateTimerService.Elapsed += (sender, e) => ++invocationCount;

            _databaseUpdateTimerService.Start();

            for (int currentInvocations = 1; currentInvocations <= expectedInvocations; ++currentInvocations)
            {
                _timerMock.Raise(timer => timer.Elapsed += null, (EventArgs)null);
                Assert.Equal(currentInvocations, invocationCount);
            }
        }

        [Fact]
        public void TestUsesUpdateTimeAsSourceTime()
        {
            _updateTime = new DateTime(2020, 1, 1, 1, 1, 1);

            _databaseUpdateTimerService.Start();

            _timeIntervalCalculatorMock.Verify(timeIntervalCalculator => timeIntervalCalculator.CalculateTimeUntilDeadline(_updateTime), Times.Once);
        }

        [Fact]
        public void TestDisposesTimerOnDispose()
        {
            _databaseUpdateTimerService.Dispose();

            _timerMock.Verify(timer => timer.Dispose(), Times.Once);
        }
    }
}