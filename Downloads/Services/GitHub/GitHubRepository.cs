namespace Downloads.Services.GitHub
{
    using Octokit;

    public class GitHubRepository : IGitHubRepository
    {
        private readonly Repository _repository;

        public GitHubRepository(Repository repository)
        {
            _repository = repository;
        }

        public string Name => _repository.Name;

        public string HtmlUrl => _repository.HtmlUrl;
    }
}