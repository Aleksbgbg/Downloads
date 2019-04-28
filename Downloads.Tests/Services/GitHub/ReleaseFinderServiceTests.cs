namespace Downloads.Tests.Services.GitHub
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Downloads.Services.GitHub;

    using global::Octokit;

    using Moq;

    using Xunit;

    public class ReleaseFinderServiceTests
    {
        private readonly Mock<IReleasesClient> _releasesClientMock;

        private readonly ReleaseFinderService _releaseFinderService;

        public ReleaseFinderServiceTests()
        {
            _releasesClientMock = new Mock<IReleasesClient>();

            Mock<IGitHubClient> gitHubClientMock = new Mock<IGitHubClient>();
            gitHubClientMock.SetupGet(gitHubClient => gitHubClient.Repository.Release)
                             .Returns(_releasesClientMock.Object);

            _releaseFinderService = new ReleaseFinderService(gitHubClientMock.Object);
        }

        [Fact]
        public async Task TestHasReleases()
        {
            IGitHubRepository repository = MakeRepository();
            SetupHasReleases(repository);

            bool hasReleases = await _releaseFinderService.HasReleases(repository);

            Assert.True(hasReleases);
        }

        [Fact]
        public async Task TestDoesNotHaveReleases()
        {
            IGitHubRepository repository = MakeRepository();
            SetupDoesNotHaveReleases(repository);

            bool hasReleases = await _releaseFinderService.HasReleases(repository);

            Assert.False(hasReleases);
        }

        [Fact]
        public async Task TestGetLatestRelease()
        {
            IGitHubRepository repository = MakeRepository();

            IGitHubRelease release = await _releaseFinderService.GetLatestRelease(repository);

            VerifyGetLatestReleaseCalled(repository);
        }

        private void VerifyGetLatestReleaseCalled(IGitHubRepository repository)
        {
            _releasesClientMock.Verify(releasesClient => releasesClient.GetLatest(repository.Id));
        }

        private void SetupHasReleases(IGitHubRepository repository)
        {
            const int releaseCount = 5;
            SetupGetAllReleases(repository, releaseCount);
        }

        private void SetupDoesNotHaveReleases(IGitHubRepository repository)
        {
            SetupGetAllReleases(repository, 0);
        }

        private void SetupGetAllReleases(IGitHubRepository repository, int length)
        {
            Mock<IReadOnlyList<Release>> releaseListMock = new Mock<IReadOnlyList<Release>>();
            releaseListMock.SetupGet(releaseList => releaseList.Count)
                           .Returns(length);

            _releasesClientMock.Setup(releasesClient => releasesClient.GetAll(repository.Id))
                               .ReturnsAsync(releaseListMock.Object);
        }

        private static IGitHubRepository MakeRepository(long id = 500)
        {
            Mock<IGitHubRepository> gitHubRepositoryMock = new Mock<IGitHubRepository>();
            gitHubRepositoryMock.SetupGet(repository => repository.Id)
                                .Returns(id);

            return gitHubRepositoryMock.Object;
        }
    }
}