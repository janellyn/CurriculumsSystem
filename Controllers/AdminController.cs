using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TaskAuthenticationAuthorization.Models;

namespace TaskAuthenticationAuthorization.Controllers
{
    [Authorize(Roles = ApplicationRoles.AdminRole)]
    public class AdminController : Controller
    {
        private readonly EducationalPlansContext _db;

        public AdminController(EducationalPlansContext context)
        {
            _db = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = _db.Userr.Include(u => u.Role).ToList();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _db.Userr.FirstOrDefaultAsync(u => u.ID == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(Userr user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = _db.Userr.Find(user.ID);
                    currentUser.RoleId = user.RoleId;
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        private bool UserExists(int id)
        {
            return _db.Userr.Any(u => u.ID == id);
        }
    }
}
