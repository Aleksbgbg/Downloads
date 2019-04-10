namespace Downloads.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Downloads.Infrastructure.Options;
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

        public async Task<App[]> GetReleasedGitHubApps()
        {
            IReadOnlyList<Repository> repositories = await _gitHubClient.Repository.GetAllForUser("Aleksbgbg");

            App[] apps = new App[repositories.Count];

            for (int repositoryIndex = 0; repositoryIndex < repositories.Count; ++repositoryIndex)
            {
                Repository repository = repositories[repositoryIndex];

                Release latestRelease = await _gitHubClient.Repository.Release.GetLatest(repository.Id);

                if (latestRelease != null)
                {
                    apps[repositoryIndex] = CreateApp(repository, latestRelease);
                }
            }

            return apps;
        }

        private static App CreateApp(Repository repository, Release latestRelease)
        {
            return new App
            {
                Description = GetAppDescription(latestRelease),
                DownloadUrl = GetAppDownloadUrl(latestRelease),
                GitHubUrl = GetGitHubUrl(repository),
                LatestVersion = GetAppVersion(latestRelease),
                LastUpdated = GetAppReleaseDate(latestRelease),
                Name = GetAppName(repository)
            };
        }

        private static string GetAppDescription(Release latestRelease)
        {
            string bodyText = latestRelease.Body;

            string[] bodyTextParagraphs = bodyText.Split("\r\n\r\n");
            string firstParagraph = bodyTextParagraphs[0];

            string[] firstParagraphLines = firstParagraph.Split("\r\n");
            string firstParagraphBody = string.Join("\r\n", firstParagraphLines.Skip(1));

            return firstParagraphBody;
        }

        private static string GetAppDownloadUrl(Release latestRelease)
        {
            return latestRelease.Assets.Single().BrowserDownloadUrl;
        }

        private static string GetGitHubUrl(Repository repository)
        {
            return repository.HtmlUrl;
        }

        private static string GetAppVersion(Release latestRelease)
        {
            return latestRelease.TagName;
        }

        private static DateTime GetAppReleaseDate(Release latestRelease)
        {
            return latestRelease.PublishedAt.Value.UtcDateTime;
        }

        private static string GetAppName(Repository repository)
        {
            return repository.Name.Replace('-', ' ');
        }
    }
}