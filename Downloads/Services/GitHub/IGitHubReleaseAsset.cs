namespace Downloads.Services.GitHub
{
    public interface IGitHubReleaseAsset
    {
        string Name { get; }

        string BrowserDownloadUrl { get; }
    }
}