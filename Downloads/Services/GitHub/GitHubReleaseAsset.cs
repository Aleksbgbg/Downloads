namespace Downloads.Services.GitHub
{
    using Octokit;

    public class GitHubReleaseAsset : IGitHubReleaseAsset
    {
        private readonly ReleaseAsset _releaseAsset;

        public GitHubReleaseAsset(ReleaseAsset releaseAsset)
        {
            _releaseAsset = releaseAsset;
        }

        public string Name => _releaseAsset.Name;

        public string BrowserDownloadUrl => _releaseAsset.BrowserDownloadUrl;
    }
}