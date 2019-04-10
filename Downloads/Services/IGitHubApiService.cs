namespace Downloads.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Downloads.Models;

    public interface IGitHubApiService
    {
        string GetAuthUrl();

        Task<IEnumerable<GitHubRepository>> GetUserRepositories();

        Task<App[]> GetReleasedGitHubApps();
    }
}