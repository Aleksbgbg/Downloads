namespace Downloads.Services.GitHub
{
    using System;
    using System.Linq;

    public class GitHubRepositoryDataProvider : IGitHubRepositoryDataProvider
    {
        private readonly IGitHubRepository _gitHubRepository;

        private readonly IGitHubRelease _latestRelease;

        public GitHubRepositoryDataProvider(IGitHubRepository gitHubRepository, IGitHubRelease latestRelease)
        {
            _gitHubRepository = gitHubRepository;
            _latestRelease = latestRelease;
        }

        public string AppName => _gitHubRepository.Name;

        public string LatestVersion => _latestRelease.TagName;

        public string Description
        {
            get
            {
                string releaseBody = _latestRelease.Body;

                int startIndex = releaseBody.IndexOf("\r\n", StringComparison.Ordinal);
                int endIndex = releaseBody.IndexOf("\r\n\r\n", StringComparison.Ordinal);

                string description = releaseBody.Substring(startIndex, endIndex - startIndex);

                return description;
            }
        }

        public string DownloadUrl => _latestRelease.Assets.First(asset => asset.Name == "Setup.exe" || asset.Name == "Release.zip").BrowserDownloadUrl;

        public string GitHubPageUrl => _gitHubRepository.HtmlUrl;

        public DateTime LastUpdated => (_latestRelease.PublishedAt ?? _latestRelease.CreatedAt).UtcDateTime;
    }
}