namespace Downloads.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Downloads.Models;

    using Octokit;

    public class GitHubApiService : IGitHubApiService
    {
        private readonly IGitHubClient _gitHubClient;

        public GitHubApiService(IGitHubClient gitHubClient)
        {
            _gitHubClient = gitHubClient;
        }

        public async Task<IEnumerable<App>> GetReleasedGitHubApps()
        {
            IReadOnlyList<Repository> repositories = await _gitHubClient.Repository.GetAllForUser("Aleksbgbg");

            List<App> apps = new List<App>();

            for (int repositoryIndex = 0; repositoryIndex < repositories.Count; ++repositoryIndex)
            {
                Repository repository = repositories[repositoryIndex];

                try
                {
                    Release latestRelease = await _gitHubClient.Repository.Release.GetLatest(repository.Id);
                    apps.Add(CreateApp(repository, latestRelease));
                }
                catch (NotFoundException)
                {
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
            return latestRelease.Assets.First(asset => asset.Name == "Setup.exe" || asset.Name == "Release.zip").BrowserDownloadUrl;
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