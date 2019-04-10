namespace Downloads.Tests.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Downloads.Infrastructure.Options;
    using Downloads.Models;
    using Downloads.Services;

    using global::Octokit;

    using Microsoft.Extensions.Options;

    using Moq;

    using Xunit;

    public class GitHubApiServiceTests
    {
        private readonly Mock<IGitHubClient> _gitHubClientMock;

        private readonly GitHubApiService _gitHubApiService;

        public GitHubApiServiceTests()
        {
            _gitHubClientMock = new Mock<IGitHubClient>();

            Mock<IOptions<OctokitOptions>> octokitOptions = new Mock<IOptions<OctokitOptions>>();
            octokitOptions.SetupGet(options => options.Value)
                          .Returns(new OctokitOptions
                          {
                              AppName = "SomeApp",
                              ClientId = "Downloads"
                          });

            _gitHubApiService = new GitHubApiService(_gitHubClientMock.Object, octokitOptions.Object);
        }

        [Fact]
        public void TestAuthUrl()
        {
            string result = _gitHubApiService.GetAuthUrl();

            Assert.Equal("https://github.com/login/oauth/authorize?client_id=Downloads", result);
        }

        [Fact]
        public async Task TestGetUserRepositories()
        {
            Mock<IRepositoriesClient> repositoryMock = new Mock<IRepositoriesClient>();
            repositoryMock.Setup(repository => repository.GetAllForUser("Aleksbgbg"))
                          .ReturnsAsync(new Repository[0]);

            _gitHubClientMock.SetupGet(gitHubClient => gitHubClient.Repository)
                             .Returns(repositoryMock.Object);

            IEnumerable<GitHubRepository> repositories = await _gitHubApiService.GetUserRepositories();

            repositoryMock.Verify(repository => repository.GetAllForUser("Aleksbgbg"), Times.Once);
            Assert.Empty(repositories);
        }
    }
}