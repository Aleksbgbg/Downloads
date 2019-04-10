namespace Downloads.Tests.Services
{
    using Downloads.Services;

    using global::Octokit;

    using Moq;

    public class GitHubApiServiceTests
    {
        private readonly Mock<IGitHubClient> _gitHubClientMock;

        private readonly GitHubApiService _gitHubApiService;

        public GitHubApiServiceTests()
        {
            _gitHubClientMock = new Mock<IGitHubClient>();
            _gitHubApiService = new GitHubApiService(_gitHubClientMock.Object);
        }
    }
}