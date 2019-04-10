namespace Downloads.Tests.Services.DatabaseUpdates
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Downloads.Models;
    using Downloads.Models.Repositories;
    using Downloads.Services;
    using Downloads.Services.DatabaseUpdates;
    using Downloads.Tests.Api;

    using Moq;

    using Xunit;

    public class AppRepositoryUpdateServiceTests
    {
        private readonly AppRepositoryUpdateService _appRepositoryUpdateService;

        private readonly Mock<IGitHubApiService> _gitHubApiServiceMock;

        private readonly Mock<IAppRepository> _appRepositoryMock;

        private App[] _gitHubApps;

        private App[] _appRepositoryApps;

        public AppRepositoryUpdateServiceTests()
        {
            _gitHubApiServiceMock = new Mock<IGitHubApiService>();
            _gitHubApiServiceMock.Setup(gitHubApiService => gitHubApiService.GetReleasedGitHubApps())
                                 .ReturnsAsync(() => _gitHubApps);

            _appRepositoryMock = new Mock<IAppRepository>();
            _appRepositoryMock.SetupGet(appRepository => appRepository.Apps)
                              .Returns(() => _appRepositoryApps.AsQueryable());

            _appRepositoryUpdateService = new AppRepositoryUpdateService(_gitHubApiServiceMock.Object, _appRepositoryMock.Object);
        }

        [Fact]
        public async Task TestUpdateFetchesAllRepositories()
        {
            _gitHubApps = new App[0];
            _appRepositoryApps = new App[0];

            await _appRepositoryUpdateService.UpdateApps();

            _gitHubApiServiceMock.Verify(gitHubApiService => gitHubApiService.GetReleasedGitHubApps(), Times.Once);
        }

        [Fact]
        public async Task TestUpdateDeletesOldRepositories()
        {
            List<App> apps = Data.Apps.ToList();
            App lastApp = apps.Last();
            apps.Remove(lastApp);
            _gitHubApps = apps.ToArray();

            _appRepositoryApps = Data.Apps;

            await _appRepositoryUpdateService.UpdateApps();

            _appRepositoryMock.Verify(appRepository => appRepository.Remove(lastApp), Times.Once);
        }

        [Fact]
        public async Task TestUpdateUpdatesOutdatedRepositories()
        {
            _gitHubApps = Data.Apps;

            List<App> apps = Data.Apps.ToList();
            App firstApp = apps[0];
            App firstAppReplacement = new App
            {
                Name = firstApp.Name,
                LatestVersion = "v0"
            };
            apps[0] = firstAppReplacement;
            _appRepositoryApps = apps.ToArray();

            await _appRepositoryUpdateService.UpdateApps();

            _appRepositoryMock.Verify(appRepository => appRepository.Update(firstAppReplacement), Times.Once);
            Assert.Equal(firstApp.DownloadUrl, firstAppReplacement.DownloadUrl);
        }

        [Fact]
        public async Task TestUpdateAddsNewRepositories()
        {
            _gitHubApps = Data.Apps;

            List<App> apps = Data.Apps.ToList();
            App firstApp = apps[0];
            apps.Remove(firstApp);
            _appRepositoryApps = apps.ToArray();

            await _appRepositoryUpdateService.UpdateApps();

            _appRepositoryMock.Verify(appRepository => appRepository.Add(firstApp), Times.Once);
        }
    }
}