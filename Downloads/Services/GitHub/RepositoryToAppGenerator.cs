namespace Downloads.Services.GitHub
{
    using Downloads.Models;

    public class RepositoryToAppGenerator
    {
        private readonly IGitHubRepositoryDataProvider _gitHubRepositoryDataProvider;

        public RepositoryToAppGenerator(IGitHubRepositoryDataProvider gitHubRepositoryDataProvider)
        {
            _gitHubRepositoryDataProvider = gitHubRepositoryDataProvider;
        }

        public App GenerateApp()
        {
            return new App
            {
                Name = _gitHubRepositoryDataProvider.AppName,
                LatestVersion = _gitHubRepositoryDataProvider.LatestVersion,
                Description = _gitHubRepositoryDataProvider.Description,
                DownloadUrl = _gitHubRepositoryDataProvider.DownloadUrl,
                GitHubUrl = _gitHubRepositoryDataProvider.GitHubPageUrl,
                LastUpdated = _gitHubRepositoryDataProvider.LastUpdated
            };
        }
    }
}