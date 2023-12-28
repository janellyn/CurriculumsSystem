using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskAuthenticationAuthorization.Models;

namespace TaskAuthenticationAuthorization.Controllers
{
    public class CabinetController : Controller
    {
        private readonly EducationalPlansContext _context;

        public CabinetController(EducationalPlansContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cabinetID = HttpContext.Session.GetInt32("CabinetId");

            var curriculum = from c in _context.Cabinet_Curriculum
                             .Include(c => c.Cabinet)
                             .Include(c => c.Curriculum)
                             .Include(c => c.Curriculum.Education_Level)
                             .Include(c => c.Curriculum.Learning_Form)
                             .Include(c => c.Curriculum.Faculty_Institute)
                             .Include(c => c.Curriculum.Speciality)
                             .Include(c => c.Curriculum.Educational_Program)
                             select c;
            if (cabinetID != null)
            {
                curriculum = curriculum.Where(s => s.CabinetID == cabinetID);
            }

            return View(await curriculum.AsNoTracking().ToListAsync());
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cab = await _context.Cabinet_Curriculum
                .FirstOrDefaultAsync(m => m.CurriculumID == id);
            if (cab == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "EducPlanDetail", new { searchInt = id });
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            var cabid = HttpContext.Session.GetInt32("CabinetId");

            if (id == null)
            {
                return NotFound();
            }

            var cur = _context.Cabinet_Curriculum
                             .Include(c => c.Cabinet)
                             .Include(c => c.Curriculum)
                             .Include(c => c.Curriculum.Education_Level)
                             .Include(c => c.Curriculum.Learning_Form)
                             .Include(c => c.Curriculum.Faculty_Institute)
                             .Include(c => c.Curriculum.Speciality)
                             .Include(c => c.Curriculum.Educational_Program)
                         .Where(c => c.CabinetID == cabid)
                         .Where(c => c.CurriculumID == id)
                         .FirstOrDefault();
            if (cur == null)
            {
                return NotFound();
            }

            return View(cur);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cabid = HttpContext.Session.GetInt32("CabinetId");
            var cur = await _context.Cabinet_Curriculum.Where(c => c.CabinetID == cabid && c.CurriculumID == id)
            .FirstOrDefaultAsync();
            _context.Cabinet_Curriculum.Remove(cur);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
