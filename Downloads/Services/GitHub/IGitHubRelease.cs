namespace Downloads.Services.GitHub
{
    using System.Collections.Generic;

    public interface IGitHubRelease
    {
        string TagName { get; }

        string Body { get; }

        IEnumerable<IGitHubReleaseAsset> Assets { get; }
    }
}