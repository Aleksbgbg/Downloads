namespace Downloads.Controllers.Api
{
    using System.Threading.Tasks;

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
        public async Task<RedirectResult> Download(string appName)
        {
            App app = _appRepository.Find(appName);
            ++app.DownloadCount;
            await _appRepository.Update(app);
            return Redirect(app.DownloadUrl);
        }
    }
}