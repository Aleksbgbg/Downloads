namespace Downloads.Tests.Models.Repositories
{
    using System.Linq;

    using Downloads.Models;
    using Downloads.Models.Database;
    using Downloads.Models.Repositories;
    using Downloads.Tests.Api;

    using Microsoft.EntityFrameworkCore;

    using Moq;

    using Xunit;

    public class AppRepositoryTests
    {
        [Fact]
        public void TestRetrieveAllApps()
        {
            Mock<AppDbContext> appDbContextMock = new Mock<AppDbContext>();

            AppRepository appRepository = new AppRepository(appDbContextMock.Object);

            IQueryable<App> result = appRepository.Apps;

            appDbContextMock.Verify(appDbContext => appDbContext.Apps, Times.Once);
        }

        [Fact]
        public void TestFindApp()
        {
            App[] apps = Data.Apps;
            App lastApp = apps.Last();

            DbSet<App> appSet = Utils.GetQueryableMockDbSet(apps.ToList());

            Mock<AppDbContext> appDbContextMock = new Mock<AppDbContext>();
            appDbContextMock.SetupGet(appDbContext => appDbContext.Apps)
                            .Returns(appSet);

            AppRepository appRepository = new AppRepository(appDbContextMock.Object);

            App result = appRepository.Find(lastApp.Name);

            Assert.Equal(lastApp, result);
        }
    }
}