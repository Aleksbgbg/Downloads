namespace Downloads.Tests.Controllers
{
    using Downloads.Controllers;
    using Downloads.Services;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using Xunit;

    public class AdminControllerTests
    {
        private Mock<IGitHubApiService> _gitHubApiServiceMock;

        private readonly AdminController _adminController;

        public AdminControllerTests()
        {
            _gitHubApiServiceMock = new Mock<IGitHubApiService>();
            _adminController = new AdminController(_gitHubApiServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Fact]
        public void TestRedirectsWhenNotAuthenticated()
        {
            const string redirectUrl = "RedirectUrl";

            _gitHubApiServiceMock.Setup(gitHubApiService => gitHubApiService.GetAuthUrl())
                                 .Returns(redirectUrl);

            IActionResult result = _adminController.Index();

            _gitHubApiServiceMock.Verify(gitHubApiService => gitHubApiService.GetAuthUrl(), Times.Once);
            Assert.IsType<RedirectResult>(result);
            Assert.Equal(redirectUrl, ((RedirectResult)result).Url);
        }
    }
}