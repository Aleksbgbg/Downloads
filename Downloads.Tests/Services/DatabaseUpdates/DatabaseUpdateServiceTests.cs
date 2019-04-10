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
        public async Task TestCallsUpdateOnStart()
        {
            await _databaseUpdateService.StartAsync();

            _appRepositoryUpdateServiceMock.Verify(appRepositoryUpdateService => appRepositoryUpdateService.UpdateApps(), Times.Once);
        }

        [Fact]
        public async Task TestStartsTimerOnStart()
        {
            await _databaseUpdateService.StartAsync();

            _databaseUpdateTimerMock.Verify(databaseUpdateTimer => databaseUpdateTimer.Start(), Times.Once);
        }

        [Fact]
        public async Task TestUpdatesAppRepositoryOnTimerElapsed()
        {
            await _databaseUpdateService.StartAsync();

            _databaseUpdateTimerMock.Raise(databaseUpdateTimer => databaseUpdateTimer.Elapsed += null, (EventArgs)null);

            _appRepositoryUpdateServiceMock.Verify(appRepositoryUpdateService => appRepositoryUpdateService.UpdateApps(), Times.Exactly(2));
        }

        [Fact]
        public async Task TestStopsTimerOnStop()
        {
            await _databaseUpdateService.StopAsync();

            _databaseUpdateTimerMock.Verify(databaseUpdateTimer => databaseUpdateTimer.Stop(), Times.Once);
        }

        [Fact]
        public async Task TestUnsubscribesFromTimerEventOnStop()
        {
            const int timerElapsedRaiseCount = 5;

            await _databaseUpdateService.StartAsync();
            await _databaseUpdateService.StopAsync();

            for (int elapsedInvocation = 0; elapsedInvocation < timerElapsedRaiseCount; elapsedInvocation++)
            {
                _databaseUpdateTimerMock.Raise(databaseUpdateTimer => databaseUpdateTimer.Elapsed += null, (EventArgs)null);
            }

            _appRepositoryUpdateServiceMock.Verify(appRepositoryUpdateService => appRepositoryUpdateService.UpdateApps(), Times.Once);
        }

        [Fact]
        public void TestDisposesTimerOnDispose()
        {
            _databaseUpdateService.Dispose();

            _databaseUpdateTimerMock.Verify(databaseUpdateTimer => databaseUpdateTimer.Dispose(), Times.Once);
        }
    }
}