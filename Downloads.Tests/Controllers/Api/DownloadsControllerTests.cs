namespace Downloads.Tests.Controllers.Api
{
    using System.Linq;

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
        public void TestFilterAppsOnDownload()
        {
            App lastApp = Data.Apps.Last();

            _appRepositoryMock.Setup(appRepository => appRepository.Find(lastApp.Name)).Returns(lastApp);

            _appController.Download(lastApp.Name);

            _appRepositoryMock.Verify(appRepository => appRepository.Find(lastApp.Name), Times.Once);
        }

        [Fact]
        public void TestRedirectOnDownload()
        {
            App app = Data.Apps.Last();

            _appRepositoryMock.Setup(appRepository => appRepository.Find(app.Name)).Returns(app);

            RedirectResult redirect = _appController.Download(app.Name);

            Assert.False(redirect.Permanent);
            Assert.Equal(app.DownloadUrl, redirect.Url);
        }
    }
}