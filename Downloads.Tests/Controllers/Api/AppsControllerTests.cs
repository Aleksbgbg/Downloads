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
            SetupApps(apps);

            ActionResult<App[]> result = _appsController.All();

            Assert.Equal(apps, result.Value);
        }

        [Fact]
        public void TestFilterAppsOnViewApp()
        {
            App lastApp = Data.Apps.Last();
            SetupFind(lastApp);

            ActionResult<App> result = _appsController.App(lastApp.Name);

            Assert.Equal(lastApp, result.Value);
        }

        [Fact]
        public void TestFilterAppsReturnsNotFoundWhenAppMissing()
        {
            ActionResult<App> result = _appsController.App("SomeMissingAppName");

            Assert.IsType<NotFoundResult>(result.Result);
        }

        private void SetupApps(App[] apps)
        {
            _appRepositoryMock.SetupGet(appRepository => appRepository.Apps)
                              .Returns(apps.AsQueryable());
        }

        private void SetupFind(App app)
        {
            _appRepositoryMock.Setup(appRepository => appRepository.Find(app.Name))
                              .Returns(app);
        }
    }
}