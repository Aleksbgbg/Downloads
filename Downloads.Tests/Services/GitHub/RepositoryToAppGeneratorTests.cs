namespace Downloads.Tests.Services.GitHub
{
    using System;

    using Downloads.Models;
    using Downloads.Services.GitHub;

    using Moq;

    using Xunit;

    public class RepositoryToAppGeneratorTests
    {
        private const string AppName = "Tornado-Player";

        private const string LatestVersion = "v1.1.1";

        private const string Description = "SomeRandomMarkdownDescription";

        private const string DownloadUrl = "SomeDownloadUrl";

        private const string GitHubUrl = "SomeGitHubUrl";

        private static readonly DateTime LastUpdated = DateTime.UnixEpoch;

        private readonly App _app;

        public RepositoryToAppGeneratorTests()
        {
            Mock<IGitHubRepositoryDataProvider> gitHubRepositoryMock = new Mock<IGitHubRepositoryDataProvider>();
            gitHubRepositoryMock.SetupGet(repository => repository.AppName)
                                .Returns(AppName);
            gitHubRepositoryMock.SetupGet(repository => repository.LatestVersion)
                                .Returns(LatestVersion);
            gitHubRepositoryMock.SetupGet(repository => repository.Description)
                                .Returns(Description);
            gitHubRepositoryMock.SetupGet(repository => repository.DownloadUrl)
                                .Returns(DownloadUrl);
            gitHubRepositoryMock.SetupGet(repository => repository.GitHubPageUrl)
                                .Returns(GitHubUrl);
            gitHubRepositoryMock.SetupGet(repository => repository.LastUpdated)
                                .Returns(LastUpdated);

            RepositoryToAppGenerator repositoryToAppGenerator = new RepositoryToAppGenerator(gitHubRepositoryMock.Object);

            _app = repositoryToAppGenerator.GenerateApp();
        }

        [Fact]
        public void TestAppName()
        {
            Assert.Equal(AppName, _app.Name);
        }

        [Fact]
        public void TestLatestVersion()
        {
            Assert.Equal(LatestVersion, _app.LatestVersion);
        }

        [Fact]
        public void TestDescription()
        {
            Assert.Equal(Description, _app.Description);
        }

        [Fact]
        public void TestDownloadUrl()
        {
            Assert.Equal(DownloadUrl, _app.DownloadUrl);
        }

        [Fact]
        public void TestGitHubUrl()
        {
            Assert.Equal(GitHubUrl, _app.GitHubUrl);
        }

        [Fact]
        public void TestLastUpdated()
        {
            Assert.Equal(LastUpdated, _app.LastUpdated);
        }
    }
}