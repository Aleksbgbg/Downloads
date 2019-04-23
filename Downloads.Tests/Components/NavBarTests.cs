namespace Downloads.Tests.Components
{
    using System.Linq;

    using Downloads.Components;
    using Downloads.Models;
    using Downloads.Models.Repositories;
    using Downloads.Tests.Api;

    using Microsoft.AspNetCore.Mvc.ViewComponents;
    using Microsoft.AspNetCore.Routing;

    using Moq;

    using Xunit;

    public class NavBarTests
    {
        private const string ControllerKey = "Controller";

        private const string ActionKey = "Action";

        private const string AllAppsAction = "All";

        private const string SpecificAppAction = "ViewApp";

        private readonly Mock<IAppRepository> _appRepositoryMock;

        private readonly NavBar _navBar;

        public NavBarTests()
        {
            _appRepositoryMock = new Mock<IAppRepository>();

            _navBar = new NavBar(_appRepositoryMock.Object)
            {
                ViewContext =
                {
                    RouteData = new RouteData
                    {
                        Values =
                        {
                            [ControllerKey] = "",
                            [ActionKey] = ""
                        }
                    }
                }
            };
        }

        [Fact]
        public void TestRecognisesAllAppsPage()
        {
            SetAction(AllAppsAction);

            NavBarViewModel navBarViewModel = InvokeAndGetModel();

            Assert.True(navBarViewModel.IsHome);
        }

        [Fact]
        public void TestRecognisesSpecificAppPage()
        {
            SetAction(SpecificAppAction);

            NavBarViewModel navBarViewModel = InvokeAndGetModel();

            Assert.False(navBarViewModel.IsHome);
        }

        [Fact]
        public void TestRetrievesAllApps()
        {
            SetAction(AllAppsAction);
            App[] apps = Data.Apps;
            SetApps(apps);

            NavBarViewModel navBarViewModel = InvokeAndGetModel();

            Assert.Equal(apps.Select(app => app.Name), navBarViewModel.AppNavs.Select(appNav => appNav.Name));
        }

        [Fact]
        public void TestSetsCorrectActiveApp()
        {
            SetAction(SpecificAppAction);
            App[] apps = Data.Apps;
            SetApps(apps);
            App targetApp = apps.Last();
            SetTargetApp(targetApp);

            NavBarViewModel navBarViewModel = InvokeAndGetModel();

            Assert.True(navBarViewModel.AppNavs.First(appNav => appNav.Name == targetApp.Name).IsActive);
        }

        private void SetApps(App[] apps)
        {
            _appRepositoryMock.SetupGet(appRepository => appRepository.Apps)
                              .Returns(apps.AsQueryable());
        }

        private void SetTargetApp(App targetApp)
        {
            _navBar.ViewBag.AppName = targetApp.Name;
        }

        private NavBarViewModel InvokeAndGetModel()
        {
            ViewViewComponentResult result = _navBar.Invoke();
            return (NavBarViewModel)result.ViewData.Model;
        }

        private void SetController(string controller)
        {
            SetRouteData(ControllerKey, controller);
        }

        private void SetAction(string action)
        {
            SetRouteData(ActionKey, action);
        }

        private void SetRouteData(string key, object value)
        {
            _navBar.ViewContext.RouteData.Values[key] = value;
        }
    }
}