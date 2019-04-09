namespace Downloads.Tests.Controllers
{
    using System.Threading.Tasks;

    using Downloads.Controllers;
    using Downloads.Models;
    using Downloads.Services;
    using Downloads.Tests.Api;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using Xunit;

    public class AdminControllerTests
    {
        private readonly Mock<IGitHubApiService> _gitHubApiServiceMock;

        private readonly Mock<IRequestCookieCollection> _cookiesMock;

        private readonly AdminController _adminController;

        public AdminControllerTests()
        {
            _gitHubApiServiceMock = new Mock<IGitHubApiService>();

            _cookiesMock = new Mock<IRequestCookieCollection>();

            Mock<HttpRequest> httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.SetupGet(httpRequest => httpRequest.Cookies)
                           .Returns(_cookiesMock.Object);

            Mock<HttpContext> httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(httpContext => httpContext.Request)
                           .Returns(httpRequestMock.Object);

            _adminController = new AdminController(_gitHubApiServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                }
            };
        }

        [Fact]
        public async Task TestIndexNotAuthenticated()
        {
            const string redirectUrl = "RedirectUrl";

            _gitHubApiServiceMock.Setup(gitHubApiService => gitHubApiService.GetAuthUrl())
                                 .Returns(redirectUrl);

            IActionResult result = await _adminController.Index();

            _gitHubApiServiceMock.Verify(gitHubApiService => gitHubApiService.GetAuthUrl(), Times.Once);

            Assert.IsType<RedirectResult>(result);

            RedirectResult redirectResult = (RedirectResult)result;
            Assert.Equal(redirectUrl, redirectResult.Url);
        }

        [Fact]
        public async Task TestIndexWhenAuthenticated()
        {
            _cookiesMock.Setup(cookies => cookies.ContainsKey("OctokitAuth"))
                        .Returns(true);

            _gitHubApiServiceMock.Setup(gitHubApiService => gitHubApiService.GetUserRepositories())
                                 .ReturnsAsync(Data.Repositories);

            IActionResult result = await _adminController.Index();

            _gitHubApiServiceMock.Verify(gitHubApiService => gitHubApiService.GetUserRepositories(), Times.Once);

            Assert.IsType<ViewResult>(result);
            ViewResult viewResult = (ViewResult)result;

            object model = viewResult.Model;
            Assert.IsType<GitHubRepository[]>(model);
            Assert.Equal(Data.Repositories, model);
        }
    }
}