namespace Downloads.Tests.Services
{
    using Downloads.Infrastructure.Octokit;
    using Downloads.Services;

    using Microsoft.Extensions.Options;

    using Moq;

    using Xunit;

    public class GitHubApiServiceTests
    {
        private readonly GitHubApiService _gitHubApiService;

        public GitHubApiServiceTests()
        {
            Mock<IOptions<OctokitOptions>> octokitOptions = new Mock<IOptions<OctokitOptions>>();
            octokitOptions.SetupGet(options => options.Value)
                          .Returns(new OctokitOptions
                          {
                              AppName = "SomeApp",
                              ClientId = "Downloads"
                          });

            _gitHubApiService = new GitHubApiService(octokitOptions.Object);
        }

        [Fact]
        public void TestAuthUrl()
        {
            string result = _gitHubApiService.GetAuthUrl();

            Assert.Equal("https://github.com/login/oauth/authorize?client_id=Downloads", result);
        }
    }
}