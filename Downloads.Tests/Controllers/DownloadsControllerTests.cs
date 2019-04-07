namespace Downloads.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

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

        public DownloadsControllerTests()
        {
            _appRepositoryMock = new Mock<IAppRepository>();
        }

        [Fact]
        public void TestRetrieveAllApps()
        {
            App[] apps = Data.Apps;

            _appRepositoryMock.SetupGet(appRepository => appRepository.Apps)
                              .Returns(apps.AsQueryable());

            DownloadsController downloadsController = new DownloadsController(_appRepositoryMock.Object);

            ViewResult result = downloadsController.All();

            Assert.IsAssignableFrom<IEnumerable<App>>(result.Model);
            Assert.Equal(apps, result.Model);
        }

        [Fact]
        public void TestFilterApps()
        {
            App lastApp = Data.Apps.Last();

            DownloadsController downloadsController = new DownloadsController(_appRepositoryMock.Object);

            ViewResult result = downloadsController.ViewApp(lastApp.Name);

            _appRepositoryMock.Verify(appRepository => appRepository.Find(lastApp.Name), Times.Once);
        }
    }
}