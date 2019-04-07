namespace Downloads.Tests.Models.Repositories
{
    using System.Linq;

    using Downloads.Models;
    using Downloads.Models.Database;
    using Downloads.Models.Repositories;
    using Downloads.Tests.Api;

    using Moq;

    using Xunit;

    public class AppRepositoryTests
    {
        private readonly Mock<AppDbContext> _appDbContextMock;

        public AppRepositoryTests()
        {
            _appDbContextMock = new Mock<AppDbContext>();
        }

        [Fact]
        public void TestRetrieveAllApps()
        {
            AppRepository appRepository = new AppRepository(_appDbContextMock.Object);

            IQueryable<App> result = appRepository.Apps;

            _appDbContextMock.Verify(appDbContext => appDbContext.Apps, Times.Once);
        }

        [Fact]
        public void TestFindApp()
        {
            App[] apps = Data.Apps;
            App lastApp = apps.Last();

            _appDbContextMock.SetupGet(appDbContext => appDbContext.Apps)
                             .Returns(Utils.GetQueryableMockDbSet(apps.ToList()));

            AppRepository appRepository = new AppRepository(_appDbContextMock.Object);

            App result = appRepository.Find(lastApp.Name);

            Assert.Equal(lastApp, result);
        }
    }
}