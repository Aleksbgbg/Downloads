namespace Downloads.Controllers
{
    using Downloads.Services;

    using Microsoft.AspNetCore.Mvc;

    public class AdminController : Controller
    {
        private readonly IGitHubApiService _gitHubApiService;

        public AdminController(IGitHubApiService gitHubApiService)
        {
            _gitHubApiService = gitHubApiService;
        }

        public IActionResult Index()
        {
            if (HttpContext.Request.Cookies.ContainsKey("OctokitAuth"))
            {
                return NotFound();
            }
            else
            {
                return Redirect(_gitHubApiService.GetAuthUrl());
            }
        }
    }
}