namespace Downloads.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Downloads.Controllers;
    using Downloads.Models;
    using Downloads.Models.Repositories;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewComponents;

    public class NavBar : ViewComponent
    {
        private const string HomeAction = nameof(DownloadsController.All);

        private readonly IAppRepository _appRepository;

        public NavBar(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        private string CurrentControllerAction => ViewContext.RouteData.Values["Action"].ToString();

        private string CurrentAppName => ViewBag.AppName;

        public ViewViewComponentResult Invoke()
        {
            bool isHome = DetermineIsHome();
            IEnumerable<NavBarViewModel.AppNavViewModel> appNavs = GetAppNavsForCurrentPage(isHome);

            return View(new NavBarViewModel(isHome, appNavs));
        }

        private bool DetermineIsHome()
        {
            return CurrentControllerAction == HomeAction;
        }

        private IEnumerable<NavBarViewModel.AppNavViewModel> GetAppNavsForCurrentPage(bool isHome)
        {
            if (isHome)
            {
                return GetAppNavsNoneSelected();
            }

            return GetAppNavsDetermineSelected();
        }

        private IEnumerable<NavBarViewModel.AppNavViewModel> GetAppNavsNoneSelected()
        {
            return GetAppNavs(isSelected: app => false);
        }

        private IEnumerable<NavBarViewModel.AppNavViewModel> GetAppNavsDetermineSelected()
        {
            string currentAppName = CurrentAppName;
            return GetAppNavs(isSelected: app => app.Name == currentAppName);
        }

        private IEnumerable<NavBarViewModel.AppNavViewModel> GetAppNavs(Predicate<App> isSelected)
        {
            return _appRepository.Apps.Select(app => new NavBarViewModel.AppNavViewModel(isSelected(app), app.Name));
        }
    }
}