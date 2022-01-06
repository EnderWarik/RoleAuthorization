using Microsoft.AspNetCore.Mvc;

namespace AuthFromRole.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
