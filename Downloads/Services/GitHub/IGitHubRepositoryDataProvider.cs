namespace Downloads.Services.GitHub
{
    public interface IGitHubRepositoryDataProvider
    {
        string AppName { get; }

        string LatestVersion { get; }

        string Description { get; }

        string DownloadUrl { get; }

        string GitHubPageUrl { get; }
    }
}