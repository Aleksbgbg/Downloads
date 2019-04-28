namespace Downloads.Services.GitHub
{
    using Downloads.Models;

    public interface IRepositoryToAppGeneratorService
    {
        App GenerateApp(IGitHubRepositoryDataProvider gitHubRepositoryDataProvider);
    }
}