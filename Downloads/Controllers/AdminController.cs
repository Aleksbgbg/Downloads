namespace Downloads.Controllers
{
    using System.Threading.Tasks;

    using Downloads.Services;

    using Microsoft.AspNetCore.Mvc;

    public class AdminController : Controller
    {
        private readonly IGitHubApiService _gitHubApiService;

        public AdminController(IGitHubApiService gitHubApiService)
        {
            _gitHubApiService = gitHubApiService;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Request.Cookies.ContainsKey("OctokitAuth"))
            {
                return View(await _gitHubApiService.GetUserRepositories());
            }
            else
            {
                return Redirect(_gitHubApiService.GetAuthUrl());
            }
        }
    }
}