namespace Downloads.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using Downloads.Controllers;
    using Downloads.Models;
    using Downloads.Models.Repositories;
    using Downloads.Tests.Api;

    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using Xunit;

    public class DownloadsControllerTests
    {
        private readonly Mock<IAppRepository> _appRepositoryMock;

        private readonly DownloadsController _downloadsController;

        public DownloadsControllerTests()
        {
            _appRepositoryMock = new Mock<IAppRepository>();
            _downloadsController = new DownloadsController(_appRepositoryMock.Object);
        }

        [Fact]
        public void TestRetrieveAllApps()
        {
            App[] apps = Data.Apps;

            _appRepositoryMock.SetupGet(appRepository => appRepository.Apps)
                              .Returns(apps.AsQueryable());

            ViewResult result = _downloadsController.All();

            Assert.IsAssignableFrom<IEnumerable<App>>(result.Model);
            Assert.Equal(apps, result.Model);
        }

        [Fact]
        public void TestFilterAppsOnViewApp()
        {
            App lastApp = Data.Apps.Last();

            ViewResult result = _downloadsController.ViewApp(lastApp.Name);

            _appRepositoryMock.Verify(appRepository => appRepository.Find(lastApp.Name), Times.Once);
        }

        [Fact]
        public void TestFilterAppsOnDownload()
        {
            App lastApp = Data.Apps.Last();

            _appRepositoryMock.Setup(appRepository => appRepository.Find(lastApp.Name))
                              .Returns(lastApp);

            _downloadsController.Download(lastApp.Name);

            _appRepositoryMock.Verify(appRepository => appRepository.Find(lastApp.Name), Times.Once);
        }

        [Fact]
        public void TestRedirectOnDownload()
        {
            App app = Data.Apps.Last();

            _appRepositoryMock.Setup(appRepository => appRepository.Find(app.Name))
                              .Returns(app);

            RedirectResult redirect = _downloadsController.Download(app.Name);

            Assert.False(redirect.Permanent);
            Assert.Equal(app.DownloadUrl, redirect.Url);
        }
    }
}