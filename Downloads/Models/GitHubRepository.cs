namespace Downloads.Models
{
    public class GitHubRepository
    {
        public GitHubRepository(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}