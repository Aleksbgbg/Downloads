namespace Downloads.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Downloads.Controllers;
    using Downloads.Models;
    using Downloads.Models.Repositories;

    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using Xunit;

    public class DownloadsControllerTests
    {
        [Fact]
        public void TestRetrievesAllApps()
        {
            App[] apps = new App[]
            {
                new App
                {
                    Name = "Some App",
                    LastUpdated = DateTime.Today
                }
            };

            Mock<IAppRepository> appRepositoryMock = new Mock<IAppRepository>();
            appRepositoryMock.SetupGet(appRepository => appRepository.Apps)
                             .Returns(apps.AsQueryable());

            DownloadsController downloadsController = new DownloadsController(appRepositoryMock.Object);

            ViewResult result = downloadsController.All();

            Assert.IsAssignableFrom<IEnumerable<App>>(result.Model);
            Assert.Equal(result.Model, apps);
        }
    }
}