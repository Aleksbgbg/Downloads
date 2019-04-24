namespace Downloads.Controllers.Api
{
    using System.Collections.Generic;
    using System.Linq;

    using Downloads.Models;
    using Downloads.Models.Repositories;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[Controller]")]
    public class AppsController : ControllerBase
    {
        private readonly IAppRepository _appRepository;

        public AppsController(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        [HttpGet("[Action]")]
        [ProducesResponseType(typeof(IEnumerable<App>), StatusCodes.Status200OK)]
        public IActionResult All()
        {
            return Ok(_appRepository.Apps.AsEnumerable());
        }

        [HttpGet("{AppName}")]
        [ProducesResponseType(typeof(App), StatusCodes.Status200OK)]
        public IActionResult App(string appName)
        {
            return Ok(_appRepository.Find(appName));
        }
    }
}