using Microsoft.AspNetCore.Mvc;

namespace HikingGroupWebApp.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
