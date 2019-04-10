namespace Downloads.Services.DatabaseUpdates
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Downloads.Models;
    using Downloads.Models.Repositories;

    public class AppRepositoryUpdateService : IAppRepositoryUpdateService
    {
        private readonly IGitHubApiService _gitHubApiService;

        private readonly IAppRepository _appRepository;

        private Dictionary<string, App> _gitHubAppsByName;

        private Dictionary<string, App> _repositoryAppsByName;

        public AppRepositoryUpdateService(IGitHubApiService gitHubApiService, IAppRepository appRepository)
        {
            _gitHubApiService = gitHubApiService;
            _appRepository = appRepository;
        }

        public async Task UpdateApps()
        {
            App[] githubApps = await _gitHubApiService.GetReleasedGitHubApps();
            _gitHubAppsByName = githubApps.ToDictionary(app => app.Name, app => app);

            await RemoveRepositoryAppsNotInGitHub();

            _repositoryAppsByName = _appRepository.Apps
                                                  .Where(app => _gitHubAppsByName.ContainsKey(app.Name))
                                                  .ToDictionary(app => app.Name, app => app);

            await UpdateOutdatedRepositoryAppsViaGitHub();
            await AddGitHubAppsNotInRepository();
        }

        private async Task RemoveRepositoryAppsNotInGitHub()
        {
            IQueryable<App> appsToRemove = _appRepository.Apps.Where(app => !_gitHubAppsByName.ContainsKey(app.Name));

            foreach (App app in appsToRemove)
            {
                await _appRepository.Remove(app);
            }
        }

        private async Task UpdateOutdatedRepositoryAppsViaGitHub()
        {
            foreach (App app in _repositoryAppsByName.Values)
            {
                App gitHubApp = _gitHubAppsByName[app.Name];

                if (app.LatestVersion != gitHubApp.LatestVersion)
                {
                    app.Description = gitHubApp.Description;
                    app.DownloadUrl = gitHubApp.DownloadUrl;
                    app.LastUpdated = gitHubApp.LastUpdated;
                    app.LatestVersion = gitHubApp.LatestVersion;

                    await _appRepository.Update(app);
                }
            }
        }

        private async Task AddGitHubAppsNotInRepository()
        {
            IEnumerable<App> newGitHubApps = _gitHubAppsByName.Values.Where(app => !_repositoryAppsByName.ContainsKey(app.Name));

            foreach (App app in newGitHubApps)
            {
                await _appRepository.Add(app);
            }
        }
    }
}