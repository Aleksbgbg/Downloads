namespace Downloads.Services.GitHub
{
    using Downloads.Models;

    public class RepositoryToAppGeneratorService : IRepositoryToAppGeneratorService
    {
        public App GenerateApp(IGitHubRepositoryDataProvider gitHubRepositoryDataProvider)
        {
            return new App
            {
                Name = gitHubRepositoryDataProvider.AppName,
                LatestVersion = gitHubRepositoryDataProvider.LatestVersion,
                Description = gitHubRepositoryDataProvider.Description,
                DownloadUrl = gitHubRepositoryDataProvider.DownloadUrl,
                GitHubUrl = gitHubRepositoryDataProvider.GitHubPageUrl,
                LastUpdated = gitHubRepositoryDataProvider.LastUpdated
            };
        }
    }
}