namespace Downloads.Models.Repositories
{
    using System.Linq;

    public interface IAppRepository
    {
        IQueryable<App> Apps { get; }
    }
}