namespace Downloads.Services.GitHub
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Octokit;

    public class GitHubRelease : IGitHubRelease
    {
        private readonly Release _release;

        public GitHubRelease(Release release)
        {
            _release = release;
        }

        public string TagName => _release.TagName;

        public string Body => _release.Body;

        public IEnumerable<IGitHubReleaseAsset> Assets => _release.Assets.Select(asset => new GitHubReleaseAsset(asset));

        public DateTimeOffset? PublishedAt => _release.PublishedAt;

        public DateTimeOffset CreatedAt => _release.CreatedAt;
    }
}