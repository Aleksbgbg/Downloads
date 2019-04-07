namespace Downloads.Controllers
{
    using Downloads.Models;
    using Downloads.Models.Repositories;

    using Microsoft.AspNetCore.Mvc;

    public class DownloadsController : Controller
    {
        private readonly IAppRepository _appRepository;

        public DownloadsController(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        public ViewResult All()
        {
            return View(_appRepository.Apps);
        }

        public ViewResult ViewApp(string appName)
        {
            return View(_appRepository.Find(appName));
        }

        public RedirectResult Download(string appName)
        {
            App app = _appRepository.Find(appName);
            return Redirect(app.DownloadUrl);
        }
    }
}