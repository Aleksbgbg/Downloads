namespace Downloads.Tests.Services.GitHub
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Downloads.Services.GitHub;

    using global::Octokit;

    using Moq;

    using Xunit;

    public class RepositoryFinderServiceTests
    {
        private const string Username = "Aleksbgbg";

        private readonly Mock<IRepositoriesClient> _repositoriesClientMock;

        private readonly RepositoryFinderService _repositoryFinderService;

        public RepositoryFinderServiceTests()
        {
            _repositoriesClientMock = new Mock<IRepositoriesClient>();

            Mock<IGitHubClient> gitHubClientMock = new Mock<IGitHubClient>();
            gitHubClientMock.SetupGet(gitHubClient => gitHubClient.Repository)
                            .Returns(_repositoriesClientMock.Object);

            _repositoryFinderService = new RepositoryFinderService(gitHubClientMock.Object);
        }

        [Fact]
        public async Task TestGetAllRepositories()
        {
            const int repositoryQuantity = 50;
            SetupRepositoryListWithQuantity(repositoryQuantity);

            IEnumerable<IGitHubRepository> repositories = await _repositoryFinderService.GetAllRepositories();

            VerifyGetAllForUsernameCalled();
            Assert.Equal(repositoryQuantity, repositories.Count());
        }

        private void SetupRepositoryListWithQuantity(int quantity)
        {
            int currentIndex = 0;

            Mock<IEnumerator<Repository>> repositoryEnumeratorMock = new Mock<IEnumerator<Repository>>();
            repositoryEnumeratorMock.Setup(repositoryEnumerator => repositoryEnumerator.MoveNext())
                                    .Returns(() =>
                                    {
                                        ++currentIndex;
                                        return currentIndex <= quantity;
                                    });

            Mock<IReadOnlyList<Repository>> repositoryListMock = new Mock<IReadOnlyList<Repository>>();
            repositoryListMock.Setup(repositoryList => repositoryList.GetEnumerator())
                              .Returns(repositoryEnumeratorMock.Object);

            _repositoriesClientMock.Setup(repositoriesClient => repositoriesClient.GetAllForUser(Username))
                                   .ReturnsAsync(repositoryListMock.Object);
        }

        private void VerifyGetAllForUsernameCalled()
        {
            _repositoriesClientMock.Verify(repositoriesClient => repositoriesClient.GetAllForUser(Username));
        }
    }
}