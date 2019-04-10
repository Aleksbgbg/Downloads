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
            IQueryable<App> result = _appRepository.Apps;

            _appDbContextMock.Verify(appDbContext => appDbContext.Apps, Times.Once);
        }

        [Fact]
        public void TestFindApp()
        {
            List<App> apps = Data.Apps.ToList();
            App lastApp = apps.Last();
            SetupAppDbSet(apps);

            App result = _appRepository.Find(lastApp.Name);

            Assert.Equal(lastApp, result);
        }

        [Fact]
        public async Task TestAddApp()
        {
            List<App> apps = new List<App>();
            App newApp = Data.Apps.Last();
            SetupAppDbSet(apps);

            await _appRepository.Add(newApp);

            Assert.Contains(newApp, apps);
            VerifyCalledDatabaseSaveChanges();
        }

        [Fact]
        public async Task TestDeleteApp()
        {
            List<App> apps = Data.Apps.ToList();
            App appToRemove = apps.Last();
            SetupAppDbSet(apps);

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

            dbSetMock.Verify(dbSet => dbSet.Update(appToUpdate), Times.Once);
            VerifyCalledDatabaseSaveChanges();
        }

        private Mock<DbSet<App>> SetupAppDbSet(ICollection<App> appCollection)
        {
            Mock<DbSet<App>> mockDbSet = Utils.GetQueryableMockDbSet(appCollection);

            _appDbContextMock.SetupGet(appDbContext => appDbContext.Apps)
                             .Returns(mockDbSet.Object);

            return mockDbSet;
        }

        private void VerifyCalledDatabaseSaveChanges()
        {
            _appDbContextMock.Verify(appDbContext => appDbContext.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}