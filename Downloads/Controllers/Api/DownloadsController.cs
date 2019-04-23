namespace Downloads.Controllers.Api
{
    using Downloads.Models;
    using Downloads.Models.Repositories;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/[Controller]")]
    public class DownloadsController : ControllerBase
    {
        private readonly IAppRepository _appRepository;

        public DownloadsController(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        [HttpGet("[Action]/{AppName}")]
        public RedirectResult Download(string appName)
        {
            App app = _appRepository.Find(appName);
            return Redirect(app.DownloadUrl);
        }
    }
}