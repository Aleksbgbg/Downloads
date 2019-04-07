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

        public ViewResult ViewApp(string app)
        {
            return View(_appRepository.Find(app));
        }

        public RedirectResult Download(string app)
        {
            App targetApp = _appRepository.Find(app);
            return Redirect(targetApp.DownloadUrl);
        }
    }
}