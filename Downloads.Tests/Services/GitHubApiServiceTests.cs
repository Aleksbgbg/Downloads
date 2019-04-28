namespace Downloads.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Downloads.Models;
    using Downloads.Services;
    using Downloads.Services.GitHub;
    using Downloads.Tests.Api;

    using Moq;
    using Moq.Language;

    using Xunit;

    public class GitHubApiServiceTests
    {
        private readonly Mock<IRepositoryFinderService> _repositoryFinderServiceMock;

        private readonly Mock<IReleaseFinderService> _releaseFinderServiceMock;

        private readonly Mock<IRepositoryToAppGeneratorService> _repositoryToAppGeneratorServiceMock;

        private readonly GitHubApiService _gitHubApiService;

        public GitHubApiServiceTests()
        {
            _repositoryFinderServiceMock = new Mock<IRepositoryFinderService>();

            _releaseFinderServiceMock = new Mock<IReleaseFinderService>();

            _repositoryToAppGeneratorServiceMock = new Mock<IRepositoryToAppGeneratorService>();

            _gitHubApiService = new GitHubApiService(_repositoryFinderServiceMock.Object, _releaseFinderServiceMock.Object, _repositoryToAppGeneratorServiceMock.Object);
        }

        [Fact]
        public async Task TestGetsAllRepositories()
        {
            IEnumerable<App> apps = await _gitHubApiService.GetReleasedGitHubApps();

            VerifyGetAllRepositoriesCalled();
        }

        [Fact]
        public async Task TestChecksRepositoriesHaveReleases()
        {
            IGitHubRepository[] repositories = SetupRepositories(2);

            IEnumerable<App> apps = await _gitHubApiService.GetReleasedGitHubApps();

            VerifyHasReleasesCalled(repositories[0]);
            VerifyHasReleasesCalled(repositories[1]);
        }

        [Fact]
        public async Task TestGetsLatestReleaseForRepositoriesWithReleases()
        {
            IGitHubRepository[] repositories = SetupRepositories(new IGitHubRepository[]
            {
                SetupRepository(hasReleases: true),
                SetupRepository(hasReleases: false),
                SetupRepository(hasReleases: true)
            });

            IEnumerable<App> apps = await _gitHubApiService.GetReleasedGitHubApps();

            VerifyGetLatestReleaseCalled(repositories[0]);
            VerifyGetLatestReleaseCalled(repositories[1], Times.Never);
            VerifyGetLatestReleaseCalled(repositories[2]);
        }

        [Fact]
        public async Task TestMapsAppsCorrectly()
        {
            App[] generatedApps = SetupApps();
            App[] releasedApps = (await _gitHubApiService.GetReleasedGitHubApps()).ToArray();

            Assert.Equal(generatedApps, releasedApps);
        }

        private App[] SetupApps()
        {
            App[] apps = Data.Apps;
            IGitHubRepository[] repositories = new IGitHubRepository[apps.Length];

            ISetupSequentialResult<App> sequenceSetup = _repositoryToAppGeneratorServiceMock.SetupSequence(appGenerator => appGenerator.GenerateApp(It.IsAny<IGitHubRepositoryDataProvider>()));

            for (int appIndex = 0; appIndex < apps.Length; appIndex++)
            {
                repositories[appIndex] = SetupRepository();
                sequenceSetup.Returns(apps[appIndex]);
            }

            SetupRepositories(repositories);

            return apps;
        }

        private IGitHubRepository[] SetupRepositories(int quantity)
        {
            IGitHubRepository[] repositories = new IGitHubRepository[quantity];

            for (int repositoryIndex = 0; repositoryIndex < quantity; repositoryIndex++)
            {
                repositories[repositoryIndex] = SetupRepository();
            }

            return SetupRepositories(repositories);
        }

        private IGitHubRepository[] SetupRepositories(IGitHubRepository[] repositories)
        {
            _repositoryFinderServiceMock.Setup(repositoryFinder => repositoryFinder.GetAllRepositories()).ReturnsAsync(repositories);

            return repositories;
        }

        private IGitHubRepository SetupRepository(bool hasReleases = true)
        {
            Mock<IGitHubRepository> gitHubRepositoryMock = new Mock<IGitHubRepository>();

            _releaseFinderServiceMock.Setup(releaseFinder => releaseFinder.HasReleases(gitHubRepositoryMock.Object)).ReturnsAsync(hasReleases);

            return gitHubRepositoryMock.Object;
        }

        private void VerifyGetAllRepositoriesCalled()
        {
            _repositoryFinderServiceMock.Verify(repositoryFinder => repositoryFinder.GetAllRepositories());
        }

        private void VerifyHasReleasesCalled(IGitHubRepository repository)
        {
            _releaseFinderServiceMock.Verify(releaseFinder => releaseFinder.HasReleases(repository));
        }

        private void VerifyGetLatestReleaseCalled(IGitHubRepository repository)
        {
            VerifyGetLatestReleaseCalled(repository, Times.Once);
        }

        private void VerifyGetLatestReleaseCalled(IGitHubRepository repository, Func<Times> times)
        {
            _releaseFinderServiceMock.Verify(releaseFinder => releaseFinder.GetLatestRelease(repository), times);
        }
    }
}