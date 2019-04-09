namespace Downloads.Tests.Octokit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using global::Octokit;

    using Xunit;

    public class OctokitTests : IAsyncLifetime
    {
        private readonly GitHubClient _gitHubClient;

        private Repository _youtubeDownloaderRepository;

        public OctokitTests()
        {
            _gitHubClient = new GitHubClient(new ProductHeaderValue("SomeApp"));
        }

        public async Task InitializeAsync()
        {
            IReadOnlyList<Repository> repositories = await _gitHubClient.Repository.GetAllForUser("Aleksbgbg");
            _youtubeDownloaderRepository = repositories.Single(repository => repository.Name == "YouTube-Downloader");
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        [Fact]
        public void TestGitHubPageUrl()
        {
            Assert.Equal("https://github.com/Aleksbgbg/YouTube-Downloader", _youtubeDownloaderRepository.HtmlUrl);
        }

        [Fact]
        public void TestAppName()
        {
            Assert.Equal("YouTube-Downloader", _youtubeDownloaderRepository.Name);
            Assert.Equal("YouTube Downloader", _youtubeDownloaderRepository.Name.Replace('-', ' '));
        }

        [Fact]
        public async Task TestAppVersion()
        {
            Release latestRelease = await GetLatestRelease();

            Assert.Equal("v1.2.0", latestRelease.TagName);
        }

        [Fact]
        public async Task TestAppVersionReleaseDate()
        {
            Release latestRelease = await GetLatestRelease();

            DateTimeOffset? releaseDate = latestRelease.PublishedAt;

            Assert.NotNull(releaseDate);
            Assert.Equal(new DateTime(2018, 6, 16, 11, 10, 36), releaseDate.Value.UtcDateTime);
        }

        [Fact]
        public async Task TestAppDescription()
        {
            const string descriptionHeader = "## Description";
            const string descriptionText = "Users can now download videos in MP4 and MP3 formats. In addition, users can exchange videos for more accurate matches.";

            Release latestRelease = await GetLatestRelease();
            string bodyText = latestRelease.Body;

            string[] bodyTextParagraphs = bodyText.Split("\r\n\r\n");
            string firstParagraph = bodyTextParagraphs[0];

            string[] firstParagraphLines = firstParagraph.Split("\r\n");
            string firstParagraphHeader = firstParagraphLines[0];
            string firstParagraphBody = string.Join("\r\n", firstParagraphLines.Skip(1));

            Assert.Equal(descriptionHeader, firstParagraphHeader);
            Assert.Equal(descriptionText, firstParagraphBody);
        }

        [Fact]
        public async Task TestFileDownloadUrl()
        {
            Release latestRelease = await GetLatestRelease();

            IReadOnlyList<ReleaseAsset> assets = latestRelease.Assets;
            ReleaseAsset releaseZipAsset = assets.Single();

            Assert.Equal("https://github.com/Aleksbgbg/YouTube-Downloader/releases/download/v1.2.0/Release.zip", releaseZipAsset.BrowserDownloadUrl);
        }

        private async Task<Release> GetLatestRelease()
        {
            IReadOnlyList<Release> releases = await _gitHubClient.Repository.Release.GetAll(_youtubeDownloaderRepository.Id);
            Release latestRelease = releases[0];
            return latestRelease;
        }
    }
}