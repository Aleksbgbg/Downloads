namespace Downloads.Tests.Services.DatabaseUpdates
{
    using System;
    using System.Threading.Tasks;

    using Downloads.Services.DatabaseUpdates;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class DatabaseUpdateServiceTests
    {
        private readonly Mock<ILogger<DatabaseUpdateService>> _loggerMock;

        private readonly Mock<IAppRepositoryUpdateService> _appRepositoryUpdateServiceMock;

        private readonly Mock<IDatabaseUpdateTimerService> _databaseUpdateTimerMock;

        private readonly DatabaseUpdateService _databaseUpdateService;

        public DatabaseUpdateServiceTests()
        {
            _loggerMock = new Mock<ILogger<DatabaseUpdateService>>();

            _appRepositoryUpdateServiceMock = new Mock<IAppRepositoryUpdateService>();

            _databaseUpdateTimerMock = new Mock<IDatabaseUpdateTimerService>();

            Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();
            Mock<IServiceScope> serviceScopeMock = new Mock<IServiceScope>();

            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAppRepositoryUpdateService)))
                               .Returns(_appRepositoryUpdateServiceMock.Object);

            serviceScopeMock.Setup(serviceScope => serviceScope.ServiceProvider)
                            .Returns(serviceProviderMock.Object);

            _databaseUpdateService = new DatabaseUpdateService(_loggerMock.Object, _databaseUpdateTimerMock.Object, serviceProviderMock.Object, serviceProvider => serviceScopeMock.Object);
        }

        [Fact]
        public async Task TestStartsTimerOnStart()
        {
            await _databaseUpdateService.StartAsync();

            VerifyStartTimer();
        }

        [Fact]
        public async Task TestUpdatesAppRepositoryOnTimerElapsed()
        {
            await _databaseUpdateService.StartAsync();

            RaiseTimerElapsedEvent();

            VerifyUpdateApps();
        }

        [Fact]
        public async Task TestStopsTimerOnStop()
        {
            await _databaseUpdateService.StopAsync();

            VerifyStopTimer();
        }

        [Fact]
        public async Task TestUnsubscribesFromTimerEventOnStop()
        {
            const int timerElapsedRaiseCount = 5;

            await _databaseUpdateService.StartAsync();
            await _databaseUpdateService.StopAsync();

            for (int elapsedInvocation = 0; elapsedInvocation < timerElapsedRaiseCount; elapsedInvocation++)
            {
                RaiseTimerElapsedEvent();
            }

            VerifyUpdateApps(Times.Never);
        }

        [Fact]
        public void TestDisposesTimerOnDispose()
        {
            _databaseUpdateService.Dispose();

            VerifyDisposeTimer();
        }

        private void RaiseTimerElapsedEvent()
        {
            _databaseUpdateTimerMock.Raise(databaseUpdateTimer => databaseUpdateTimer.Elapsed += null, (EventArgs)null);
        }

        private void VerifyStartTimer()
        {
            _databaseUpdateTimerMock.Verify(databaseUpdateTimer => databaseUpdateTimer.Start());
        }

        private void VerifyStopTimer()
        {
            _databaseUpdateTimerMock.Verify(databaseUpdateTimer => databaseUpdateTimer.Stop());
        }

        private void VerifyDisposeTimer()
        {
            _databaseUpdateTimerMock.Verify(databaseUpdateTimer => databaseUpdateTimer.Dispose());
        }

        private void VerifyUpdateApps()
        {
            VerifyUpdateApps(Times.Once);
        }

        private void VerifyUpdateApps(Func<Times> times)
        {
            _appRepositoryUpdateServiceMock.Verify(appRepositoryUpdateService => appRepositoryUpdateService.UpdateApps(), times);
        }
    }
}