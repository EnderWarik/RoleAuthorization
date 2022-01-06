using Microsoft.AspNetCore.Mvc;

namespace AuthFromRole.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
