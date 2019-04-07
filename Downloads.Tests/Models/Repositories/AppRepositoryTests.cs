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
            App lastApp = Data.Apps.Last();

            _appDbContextMock.SetupGet(appDbContext => appDbContext.Apps)
                             .Returns(Utils.GetQueryableMockDbSet(Data.Apps.ToList()));

            App result = _appRepository.Find(lastApp.Name);

            Assert.Equal(lastApp, result);
        }
    }
}