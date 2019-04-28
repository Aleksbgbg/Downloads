namespace Downloads.Services.GitHub
{
    public interface IGitHubRepository
    {
        string Name { get; }

        string HtmlUrl { get; }
    }
}