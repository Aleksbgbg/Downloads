namespace Downloads.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Downloads.Infrastructure.Octokit;
    using Downloads.Models;

    using Microsoft.Extensions.Options;

    using Octokit;

    public class GitHubApiService : IGitHubApiService
    {
        private readonly IGitHubClient _gitHubClient;

        private readonly OctokitOptions _octokitOptions;

        public GitHubApiService(IGitHubClient gitHubClient, IOptions<OctokitOptions> octokitOptions)
        {
            _octokitOptions = octokitOptions.Value;
            _gitHubClient = gitHubClient;
        }

        public string GetAuthUrl()
        {
            OauthLoginRequest request = new OauthLoginRequest(_octokitOptions.ClientId);
            return _gitHubClient.Oauth.GetGitHubLoginUrl(request).ToString();
        }

        public async Task<IEnumerable<GitHubRepository>> GetUserRepositories()
        {
            IReadOnlyList<Repository> repositories = await _gitHubClient.Repository.GetAllForUser("Aleksbgbg");
            return repositories.Select(repository => new GitHubRepository(repository.Name));
        }
    }
}