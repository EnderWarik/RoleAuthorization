using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthFromRole.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace AuthFromRole.Controllers
{
    public class AdminController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new User());

        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(User model)
        {
            bool AlredyExist = false;
            //if (!ModelState.IsValid)
            //{
            //    Console.WriteLine("Modelnotvalid");
            //    return View(model);
            //}
            using AuthFromRole.Models.ApplicationContext db = new AuthFromRole.Models.ApplicationContext();
            {
               
                foreach (var l in db.user.ToList())
                {
                    Console.WriteLine(l.Name);
                    Console.WriteLine(l.Password);
                    if (model.Name == l.Name && model.Password == l.Password)
                    {
                        
                         AlredyExist = true;

                    }
                }
            }
            Console.WriteLine(AlredyExist);
            if (AlredyExist)
            {
                Console.WriteLine("AlreadyExistss");
                var claims = new List<Claim>
                            {
                               new Claim("Demo","Value")
                             };
                var claimIdentity = new ClaimsIdentity(claims, "Cookie");
                var claimPrincipal = new ClaimsPrincipal(claimIdentity);
                await HttpContext.SignInAsync("Cookie", claimPrincipal);
                return Redirect(model.ReturnUrl);
            }


            
            return View("Login");
        }

    }
}
