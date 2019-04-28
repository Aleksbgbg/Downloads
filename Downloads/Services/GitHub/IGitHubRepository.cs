namespace Downloads.Services.GitHub
{
    public interface IGitHubRepository
    {
        long Id { get; }

        string Name { get; }

        string HtmlUrl { get; }
    }
}