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

    public class AppControllerTests
    {
        private readonly Mock<IAppRepository> _appRepositoryMock;

        private readonly AppController _appController;

        public AppControllerTests()
        {
            _appRepositoryMock = new Mock<IAppRepository>();
            _appController = new AppController(_appRepositoryMock.Object);
        }

        [Fact]
        public void TestRetrieveAllApps()
        {
            App[] apps = Data.Apps;

            _appRepositoryMock.SetupGet(appRepository => appRepository.Apps)
                              .Returns(apps.AsQueryable());

            ViewResult result = _appController.All();

            Assert.IsAssignableFrom<IEnumerable<App>>(result.Model);
            Assert.Equal(apps, result.Model);
        }

        [Fact]
        public void TestFilterAppsOnViewApp()
        {
            App lastApp = Data.Apps.Last();

            ViewResult result = _appController.ViewApp(lastApp.Name);

            _appRepositoryMock.Verify(appRepository => appRepository.Find(lastApp.Name), Times.Once);
        }
    }
}