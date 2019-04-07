namespace Downloads.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class DownloadsController : Controller
    {
        public ViewResult All()
        {
            return View();
        }
    }
}