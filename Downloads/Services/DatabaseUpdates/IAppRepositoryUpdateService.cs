namespace Downloads.Services.DatabaseUpdates
{
    using System.Threading.Tasks;

    public interface IAppRepositoryUpdateService
    {
        Task UpdateApps();
    }
}