namespace Downloads.Services.GitHub
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Octokit;

    public class RepositoryFinderService : IRepositoryFinderService
    {
        private const string Username = "Aleksbgbg";

        private readonly IGitHubClient _gitHubClient;

        public RepositoryFinderService(IGitHubClient gitHubClient)
        {
            _gitHubClient = gitHubClient;
        }

        public async Task<IEnumerable<IGitHubRepository>> GetAllRepositories()
        {
            IReadOnlyList<Repository> repositories = await _gitHubClient.Repository.GetAllForUser(Username);

            return repositories.Select(repository => new GitHubRepository(repository));
        }
    }
}