namespace Downloads.Controllers
{
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

        public void Download(string app)
        {
        }
    }
}