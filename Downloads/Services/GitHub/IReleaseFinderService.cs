namespace Downloads.Services.GitHub
{
    using System.Threading.Tasks;

    public interface IReleaseFinderService
    {
        Task<bool> HasReleases(IGitHubRepository repository);

        Task<IGitHubRelease> GetLatestRelease(IGitHubRepository repository);
    }
}