namespace Downloads.Services.GitHub
{
    using System;

    public interface IGitHubRepositoryDataProvider
    {
        string AppName { get; }

        string LatestVersion { get; }

        string Description { get; }

        string DownloadUrl { get; }

        string GitHubPageUrl { get; }

        DateTime LastUpdated { get; }
    }
}