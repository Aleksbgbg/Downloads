namespace Downloads.Controllers.Api
{
    using System.Linq;

    using Downloads.Models;
    using Downloads.Models.Repositories;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[Controller]/[Action]")]
    public class AppsController : ControllerBase
    {
        private readonly IAppRepository _appRepository;

        public AppsController(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<App[]> All()
        {
            return _appRepository.Apps.ToArray();
        }

        [HttpGet("{appName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<App> App(string appName)
        {
            App app = _appRepository.Find(appName);

            if (app == null)
            {
                return NotFound();
            }

            return app;
        }
    }
}