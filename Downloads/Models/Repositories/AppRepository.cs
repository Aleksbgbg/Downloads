namespace Downloads.Models.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    using Downloads.Models.Database;

    public class AppRepository : IAppRepository
    {
        private readonly AppDbContext _appDbContext;

        public AppRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IQueryable<App> Apps => _appDbContext.Apps;

        public App Find(string appName)
        {
            return Apps.Single(app => app.Name == appName);
        }

        public async Task Add(App newApp)
        {
            _appDbContext.Apps.Add(newApp);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Remove(App app)
        {
            _appDbContext.Apps.Remove(app);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Update(App app)
        {
            _appDbContext.Apps.Update(app);
            await _appDbContext.SaveChangesAsync();
        }
    }
}