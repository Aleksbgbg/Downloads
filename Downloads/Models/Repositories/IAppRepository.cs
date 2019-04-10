namespace Downloads.Models.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IAppRepository
    {
        IQueryable<App> Apps { get; }

        App Find(string appName);

        Task Add(App newApp);

        Task Remove(App app);

        Task Update(App app);
    }
}