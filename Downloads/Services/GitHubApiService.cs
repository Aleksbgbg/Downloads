namespace Downloads.Services
{
    using System.Collections.Generic;
    using System.Linq;
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
            IGitHubRepository[] repositories = (await _repositoryFinderService.GetAllRepositories()).ToArray();

            App[] apps = new App[repositories.Length];

            for (int repositoryIndex = 0; repositoryIndex < repositories.Length; repositoryIndex++)
            {
                IGitHubRepository repository = repositories[repositoryIndex];

                bool hasReleases = await _releaseFinderService.HasReleases(repository);

                if (hasReleases)
                {
                    IGitHubRelease latestRelease = await _releaseFinderService.GetLatestRelease(repository);

                    App repositoryApp = _repositoryToAppGeneratorService.GenerateApp(new GitHubRepositoryDataProvider(repository, latestRelease));

                    apps[repositoryIndex] = repositoryApp;
                }
            }

            return apps;
        }
    }
}