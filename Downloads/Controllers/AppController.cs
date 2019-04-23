namespace Downloads.Controllers
{
    using Downloads.Models.Repositories;

    using Microsoft.AspNetCore.Mvc;

    public class AppController : Controller
    {
        private readonly IAppRepository _appRepository;

        public AppController(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        public ViewResult All()
        {
            return View(_appRepository.Apps);
        }

        public ViewResult ViewApp(string appName)
        {
            ViewBag.AppName = appName;
            return View(_appRepository.Find(appName));
        }
    }
}