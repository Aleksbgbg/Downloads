namespace Downloads.Tests.Models.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Downloads.Models;
    using Downloads.Models.Database;
    using Downloads.Models.Repositories;
    using Downloads.Tests.Api;

    using Microsoft.EntityFrameworkCore;

    using Moq;

    using Xunit;

    public class AppRepositoryTests
    {
        private readonly Mock<AppDbContext> _appDbContextMock;

        private readonly AppRepository _appRepository;

        public AppRepositoryTests()
        {
            _appDbContextMock = new Mock<AppDbContext>();
            _appRepository = new AppRepository(_appDbContextMock.Object);
        }

        [Fact]
        public void TestRetrieveAllApps()
        {
            SetupAppList();

            var result = _appRepository.Apps;

            VerifyRetrievedAppsFromDb();
        }

        [Fact]
        public void TestAppsInOrder()
        {
            List<App> apps = SetupAppList();
            App lastAlphabetIndexApp = new App
            {
                Name = "ZZZZ"
            };
            apps.Insert(0, lastAlphabetIndexApp);

            IQueryable<App> result = _appRepository.Apps;

            Assert.Equal(lastAlphabetIndexApp, result.Last());
        }

        [Fact]
        public void TestFindApp()
        {
            List<App> apps = SetupAppList();
            App lastApp = apps.Last();

            App result = _appRepository.Find(lastApp.Name);

            Assert.Equal(lastApp, result);
        }

        [Fact]
        public async Task TestAddApp()
        {
            List<App> apps = SetupEmptyAppList();
            App newApp = Data.Apps.Last();

            await _appRepository.Add(newApp);

            Assert.Contains(newApp, apps);
            VerifyCalledDatabaseSaveChanges();
        }

        [Fact]
        public async Task TestDeleteApp()
        {
            List<App> apps = SetupAppList();
            App appToRemove = apps.Last();

            await _appRepository.Remove(appToRemove);

            Assert.DoesNotContain(appToRemove, apps);
            VerifyCalledDatabaseSaveChanges();
        }

        [Fact]
        public async Task TestUpdateApp()
        {
            List<App> apps = Data.Apps.ToList();
            App appToUpdate = apps.Last();
            Mock<DbSet<App>> dbSetMock = SetupAppDbSet(apps);

            await _appRepository.Update(appToUpdate);

            VerifyUpdateDbSetWithApp(dbSetMock, appToUpdate);
            VerifyCalledDatabaseSaveChanges();
        }

        private List<App> SetupAppList()
        {
            List<App> apps = Data.Apps.ToList();
            SetupAppDbSet(apps);

            return apps;
        }

        private List<App> SetupEmptyAppList()
        {
            List<App> apps = new List<App>();
            SetupAppDbSet(apps);

            return apps;
        }

        private Mock<DbSet<App>> SetupAppDbSet(ICollection<App> appCollection)
        {
            Mock<DbSet<App>> mockDbSet = Utils.GetQueryableMockDbSet(appCollection);

            _appDbContextMock.SetupGet(appDbContext => appDbContext.Apps)
                             .Returns(mockDbSet.Object);

            return mockDbSet;
        }

        private void VerifyUpdateDbSetWithApp(Mock<DbSet<App>> dbSetMock, App appToUpdate)
        {
            dbSetMock.Verify(dbSet => dbSet.Update(appToUpdate));
        }

        private void VerifyRetrievedAppsFromDb()
        {
            _appDbContextMock.Verify(appDbContext => appDbContext.Apps);
        }

        private void VerifyCalledDatabaseSaveChanges()
        {
            _appDbContextMock.Verify(appDbContext => appDbContext.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }
    }
}