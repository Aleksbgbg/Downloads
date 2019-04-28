namespace Downloads.Services.GitHub
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Octokit;

    public class ReleaseFinderService : IReleaseFinderService
    {
        private readonly IGitHubClient _gitHubClient;

        public ReleaseFinderService(IGitHubClient gitHubClient)
        {
            _gitHubClient = gitHubClient;
        }

        public async Task<bool> HasReleases(IGitHubRepository repository)
        {
            IReadOnlyList<Release> releases = await _gitHubClient.Repository.Release.GetAll(repository.Id);

            return releases.Count > 0;
        }

        public async Task<IGitHubRelease> GetLatestRelease(IGitHubRepository repository)
        {
            return new GitHubRelease(await _gitHubClient.Repository.Release.GetLatest(repository.Id));
        }
    }
}