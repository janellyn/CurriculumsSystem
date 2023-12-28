using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskAuthenticationAuthorization.Models;
using Microsoft.AspNetCore.Identity;

namespace TaskAuthenticationAuthorization.Controllers
{
    public class AccountController : Controller
    {
        private readonly EducationalPlansContext db;
        public AccountController(EducationalPlansContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) 
            {
                Userr user = await db.Userr.FirstOrDefaultAsync(u => u.Login == model.Login);
                if (user == null)
                {
                    // adding user to DB
                    var newUser = new Userr { Login = model.Login, Password = model.Password };

                    Role userRole = await db.Role.FirstOrDefaultAsync(u => u.Name == "User");
                    if (userRole != null)
                    {
                        newUser.Role = userRole;
                    }

                    db.Userr.Add(newUser);
                    await db.SaveChangesAsync();

                    await Authenticate(newUser); // authentication

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Incorrect login and(or) password");
            }
            return View(model);
        }

        private async Task Authenticate(Userr user)
        {
            // creating one claim
            var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.NameIdentifier, user.ID.ToString() ),
                 new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                 new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            // creating ClaimsIdentity object
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // setting authenticational cookies
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Userr user = await db.Userr.Include(u => u.Role).FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
                if (user != null)
                {    await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                         new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                         new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                         new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name),
                    }, "ApplicationCookie")));

                    var cabinetID = user.CabinetId;
                    HttpContext.Session.SetInt32("CabinetId", cabinetID);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Incorrect login and(or) password");
            }
            return View(model);
        }
    }
}

