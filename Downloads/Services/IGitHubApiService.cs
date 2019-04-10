namespace Downloads.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Downloads.Models;

    public interface IGitHubApiService
    {
        Task<IEnumerable<App>> GetReleasedGitHubApps();
    }
}