namespace Downloads.Tests.Controllers.Api
{
    using System.Linq;
    using System.Threading.Tasks;

    using Downloads.Controllers.Api;
    using Downloads.Models;
    using Downloads.Models.Repositories;
    using Downloads.Tests.Api;

    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using Xunit;

    public class DownloadsControllerTests
    {
        private readonly Mock<IAppRepository> _appRepositoryMock;

        private readonly DownloadsController _appController;

        public DownloadsControllerTests()
        {
            _appRepositoryMock = new Mock<IAppRepository>();

            _appController = new DownloadsController(_appRepositoryMock.Object);
        }

        [Fact]
        public async Task TestFilterAppsOnDownload()
        {
            App lastApp = Data.Apps.Last();
            SetupFindApp(lastApp);

            await _appController.Download(lastApp.Name);

            VerifySearchedApp(lastApp);
        }

        [Fact]
        public async Task TestRedirectOnDownload()
        {
            App app = Data.Apps.Last();
            SetupFindApp(app);

            RedirectResult redirect = await _appController.Download(app.Name);

            Assert.False(redirect.Permanent);
            Assert.Equal(app.DownloadUrl, redirect.Url);
        }

        [Fact]
        public async Task TestIncrementAppDownloadsOnDownload()
        {
            App app = new App
            {
                DownloadUrl = "SomeDownloadUrl",
                DownloadCount = 12
            };
            SetupFindApp(app);

            await _appController.Download(app.Name);

            Assert.Equal(13, app.DownloadCount);
            VerifyUpdateApp(app);
        }

        private void SetupFindApp(App app)
        {
            _appRepositoryMock.Setup(appRepository => appRepository.Find(app.Name))
                              .Returns(app);
        }

        private void VerifySearchedApp(App app)
        {
            _appRepositoryMock.Verify(appRepository => appRepository.Find(app.Name));
        }

        private void VerifyUpdateApp(App app)
        {
            _appRepositoryMock.Verify(appRepository => appRepository.Update(app));
        }
    }
}