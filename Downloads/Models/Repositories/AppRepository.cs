namespace Downloads.Models.Repositories
{
    using System.Linq;

    using Downloads.Models.Database;

    public class AppRepository : IAppRepository
    {
        private readonly AppDbContext _appDbContext;

        public AppRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IQueryable<App> Apps => _appDbContext.Apps;
    }
}