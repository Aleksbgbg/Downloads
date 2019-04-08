namespace Downloads.Services
{
    using Downloads.Infrastructure.Octokit;

    using Microsoft.Extensions.Options;

    using Octokit;

    public class GitHubApiService : IGitHubApiService
    {
        private readonly OctokitOptions _octokitOptions;

        private readonly GitHubClient _gitHubClient;

        public GitHubApiService(IOptions<OctokitOptions> octokitOptions)
        {
            _octokitOptions = octokitOptions.Value;
            _gitHubClient = new GitHubClient(new ProductHeaderValue(_octokitOptions.AppName));
        }

        public string GetAuthUrl()
        {
            OauthLoginRequest request = new OauthLoginRequest(_octokitOptions.ClientId);
            return _gitHubClient.Oauth.GetGitHubLoginUrl(request).ToString();
        }
    }
}