using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthFromRole.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace AuthFromRole.Controllers
{
    public class AdminController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Admin")]
        public IActionResult AdminIndex()
        {
            return View();
        }

        [Authorize(Policy = "User")]
        public IActionResult UserIndex()
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
            Console.WriteLine(model.Name);
            Console.WriteLine(model.Password);
            Console.WriteLine(GetHashString(model.Password));

            using AuthFromRole.Models.ApplicationContext db = new AuthFromRole.Models.ApplicationContext();
            {
               
                foreach (var l in db.user.ToList())
                {
                    Console.WriteLine(l.Name);
                    Console.WriteLine(l.Password);
                    //Console.WriteLine(l.Name);
                    //Console.WriteLine(l.Password);
                    if (model.Name == l.Name && GetHashString(model.Password) == l.Password)
                    {
                        
                         await CreateClaims(model, l.Role);
                        return Redirect(model.ReturnUrl);
                    }
                }
            }



            
            return Redirect("Register");
        }
        private async Task<IActionResult> CreateClaims(User model,string role)
        {
            var claims = new List<Claim>
                            {
                               new Claim(ClaimTypes.Name,model.Name),
                                new Claim(ClaimTypes.Role,role)
                             };
            var claimIdentity = new ClaimsIdentity(claims, "Cookie");
            var claimPrincipal = new ClaimsPrincipal(claimIdentity);
            await HttpContext.SignInAsync("Cookie", claimPrincipal);
            return Redirect(model.ReturnUrl);

        }

        [AllowAnonymous]
        public IActionResult Register(string returnUrl)
        {
            
                return View(new User());

        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(User model)
        {
            using AuthFromRole.Models.ApplicationContext db = new AuthFromRole.Models.ApplicationContext();
            {
                model.ReturnUrl = "/Home/";
                Console.WriteLine(model.Password);
                model.Password = GetHashString(model.Password);
                Console.WriteLine(model.Password);

                db.user.Add(model);
                db.SaveChanges();

            }
            
             CreateClaims(model, model.Role);
            return Redirect(model.ReturnUrl);

        }



        [Authorize]
        public IActionResult LogOff()
        {
            HttpContext.SignOutAsync("Cookie");
            return Redirect("/Home/Index");
        }
        string GetHashString(string s)
        {
            //переводим строку в байт-массим
            byte[] bytes = Encoding.Unicode.GetBytes(s);

            //создаем объект для получения средст шифрования
            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider();

            //вычисляем хеш-представление в байтах
            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            //формируем одну цельную строку из массива
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return hash;
        }

        //private static string HashPassword(string password)
        //{
        //    byte[] salt;
        //    byte[] buffer2;
        //    //if (password == null)
        //    //{
        //    //    throw new ArgumentNullException("password");
        //    //}
        //    using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
        //    {
        //        salt = bytes.Salt;
        //        buffer2 = bytes.GetBytes(0x20);
        //    }
        //    byte[] dst = new byte[0x31];
        //    Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
        //    Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
        //    return Convert.ToBase64String(dst);
        //}
    }
}
