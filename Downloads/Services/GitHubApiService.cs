namespace Downloads.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Downloads.Models;
    using Downloads.Services.GitHub;

    public class GitHubApiService : IGitHubApiService
    {
        private readonly IRepositoryFinderService _repositoryFinderService;

        private readonly IReleaseFinderService _releaseFinderService;

        private readonly IRepositoryToAppGeneratorService _repositoryToAppGeneratorService;

        public GitHubApiService(IRepositoryFinderService repositoryFinderService, IReleaseFinderService releaseFinderService, IRepositoryToAppGeneratorService repositoryToAppGeneratorService)
        {
            _repositoryFinderService = repositoryFinderService;
            _releaseFinderService = releaseFinderService;
            _repositoryToAppGeneratorService = repositoryToAppGeneratorService;
        }

        public async Task<IEnumerable<App>> GetReleasedGitHubApps()
        {
            IEnumerable<IGitHubRepository> repositories = await _repositoryFinderService.GetAllRepositories();

            List<App> apps = new List<App>();

            foreach (IGitHubRepository repository in repositories)
            {
                if (await HasReleases(repository))
                {
                    apps.Add(await CreateAppForRepository(repository));
                }
            }

            return apps;
        }

        private Task<bool> HasReleases(IGitHubRepository repository)
        {
            return _releaseFinderService.HasReleases(repository);
        }

        private async Task<App> CreateAppForRepository(IGitHubRepository repository)
        {
            IGitHubRelease latestRelease = await _releaseFinderService.GetLatestRelease(repository);

            App repositoryApp = _repositoryToAppGeneratorService.GenerateApp(new GitHubRepositoryDataProvider(repository, latestRelease));

            return repositoryApp;
        }
    }
}