namespace Downloads.Tests.Services.GitHub
{
    using System;

    using Downloads.Services.GitHub;

    using Moq;

    using Xunit;

    public class GitHubRepositoryDataProviderTests
    {
        private readonly Mock<IGitHubRepository> _repositoryMock;

        private readonly Mock<IGitHubRelease> _latestReleaseMock;

        private readonly GitHubRepositoryDataProvider _gitHubRepositoryDataProvider;

        public GitHubRepositoryDataProviderTests()
        {
            _repositoryMock = new Mock<IGitHubRepository>();

            _latestReleaseMock = new Mock<IGitHubRelease>();

            _gitHubRepositoryDataProvider = new GitHubRepositoryDataProvider(_repositoryMock.Object, _latestReleaseMock.Object);
        }

        [Fact]
        public void TestAppName()
        {
            const string appName = "Tornado-Player";

            SetupAppName(appName);

            Assert.Equal(appName.Replace('-', ' '), _gitHubRepositoryDataProvider.AppName);
        }

        [Fact]
        public void TestLatestVersion()
        {
            const string latestVersion = "v1.1.1";

            SetupLatestVersion(latestVersion);

            Assert.Equal(latestVersion, _gitHubRepositoryDataProvider.LatestVersion);
        }

        [Fact]
        public void TestDescription()
        {
            const string releaseBody = "## Description\r\nSome description.\r\n\r\n## Setup\r\nDownload `Setup.exe`.";
            SetupReleaseBody(releaseBody);
            string description = ParseDescription(releaseBody);

            Assert.Equal(description, _gitHubRepositoryDataProvider.Description);
        }

        [Fact]
        public void TestDescriptionNoNewlines()
        {
            const string releaseBody = "Some description.";
            SetupReleaseBody(releaseBody);

            Assert.Equal(releaseBody, _gitHubRepositoryDataProvider.Description);
        }

        [Fact]
        public void TestDownloadUrl()
        {
            const string downloadUrl = "https://download.com";

            SetupDownloadAssetsWithUrl("Setup.exe", downloadUrl);

            Assert.Equal(downloadUrl, _gitHubRepositoryDataProvider.DownloadUrl);
        }

        [Fact]
        public void TestDownloadUrlLegacySupportForZipFile()
        {
            const string downloadUrl = "https://download.com";

            SetupDownloadAssetsWithUrl("Release.zip", downloadUrl);

            Assert.Equal(downloadUrl, _gitHubRepositoryDataProvider.DownloadUrl);
        }

        [Fact]
        public void TestGitHubPageUrl()
        {
            const string gitHubPageUrl = "https://github.com/SomeUser/SomeApp";

            SetupGitHubPageUrl(gitHubPageUrl);

            Assert.Equal(gitHubPageUrl, _gitHubRepositoryDataProvider.GitHubPageUrl);
        }

        [Fact]
        public void TestLastUpdated()
        {
            DateTimeOffset utcNow = DateTime.UtcNow;
            DateTimeOffset lastUpdated = utcNow.ToOffset(TimeSpan.FromMinutes(30));
            SetupLastUpdated(lastUpdated);

            Assert.Equal(utcNow, _gitHubRepositoryDataProvider.LastUpdated);
        }

        [Fact]
        public void TestLastUpdatedFallbackToCreatedAt()
        {
            DateTimeOffset utcNow = DateTime.UtcNow;
            DateTimeOffset createdAt = utcNow.ToOffset(TimeSpan.FromMinutes(30));

            SetupLastUpdated(null);
            SetupCreatedAt(createdAt);

            Assert.Equal(createdAt, _gitHubRepositoryDataProvider.LastUpdated);
        }

        private void SetupAppName(string name)
        {
            _repositoryMock.SetupGet(repository => repository.Name)
                           .Returns(name);
        }

        private void SetupLatestVersion(string latestVersion)
        {
            _latestReleaseMock.SetupGet(release => release.TagName)
                              .Returns(latestVersion);
        }

        private string ParseDescription(string releaseBody)
        {
            int startIndex = releaseBody.IndexOf("\r\n", StringComparison.Ordinal);
            int endIndex = releaseBody.IndexOf("\r\n\r\n", StringComparison.Ordinal);

            string description = releaseBody.Substring(startIndex, endIndex - startIndex);

            return description;
        }

        private void SetupReleaseBody(string releaseBody)
        {
            _latestReleaseMock.SetupGet(release => release.Body)
                              .Returns(releaseBody);
        }

        private void SetupDownloadAssetsWithUrl(string filename, string downloadUrl)
        {
            _latestReleaseMock.SetupGet(release => release.Assets)
                              .Returns(new IGitHubReleaseAsset[]
                              {
                                  CreateAsset("App.1.1.1.nupkg"),
                                  CreateAsset(filename, downloadUrl),
                                  CreateAsset("RELEASES")
                              });
        }

        private void SetupGitHubPageUrl(string url)
        {
            _repositoryMock.SetupGet(repository => repository.HtmlUrl)
                           .Returns(url);
        }

        private void SetupLastUpdated(DateTimeOffset? lastUpdated)
        {
            _latestReleaseMock.SetupGet(release => release.PublishedAt)
                              .Returns(lastUpdated);
        }

        private void SetupCreatedAt(DateTimeOffset createdAt)
        {
            _latestReleaseMock.SetupGet(release => release.CreatedAt)
                              .Returns(createdAt);
        }

        private static IGitHubReleaseAsset CreateAsset(string name, string downloadUrl = "")
        {
            Mock<IGitHubReleaseAsset> releaseAssetMock = new Mock<IGitHubReleaseAsset>();
            releaseAssetMock.SetupGet(releaseAsset => releaseAsset.Name)
                            .Returns(name);
            releaseAssetMock.SetupGet(releaseAsset => releaseAsset.BrowserDownloadUrl)
                            .Returns(downloadUrl);

            return releaseAssetMock.Object;
        }
    }
}