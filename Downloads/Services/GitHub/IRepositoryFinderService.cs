namespace Downloads.Services.GitHub
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepositoryFinderService
    {
        Task<IEnumerable<IGitHubRepository>> GetAllRepositories();
    }
}