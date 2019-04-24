namespace Downloads.Tests.Controllers.Api
{
    using System.Collections.Generic;
    using System.Linq;

    using Downloads.Controllers.Api;
    using Downloads.Models;
    using Downloads.Models.Repositories;
    using Downloads.Tests.Api;

    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using Xunit;

    public class AppsControllerTests
    {
        private readonly Mock<IAppRepository> _appRepositoryMock;

        private readonly AppsController _appsController;

        public AppsControllerTests()
        {
            _appRepositoryMock = new Mock<IAppRepository>();
            _appsController = new AppsController(_appRepositoryMock.Object);
        }

        [Fact]
        public void TestRetrieveAllApps()
        {
            App[] apps = Data.Apps;

            _appRepositoryMock.SetupGet(appRepository => appRepository.Apps)
                              .Returns(apps.AsQueryable());

            IActionResult result = _appsController.All();

            Assert.IsAssignableFrom<IEnumerable<App>>(result);
            //Assert.Equal(apps, result);
        }

        [Fact]
        public void TestFilterAppsOnViewApp()
        {
            App lastApp = Data.Apps.Last();

            IActionResult result = _appsController.App(lastApp.Name);

            _appRepositoryMock.Verify(appRepository => appRepository.Find(lastApp.Name), Times.Once);
        }
    }
}