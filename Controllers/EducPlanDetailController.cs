using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskAuthenticationAuthorization.Models;

namespace TaskAuthenticationAuthorization.Controllers
{
    public class EducPlanDetailController : Controller
    {
        private readonly EducationalPlansContext _context;

        public EducPlanDetailController(EducationalPlansContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchInt, string searchString)
        { 
            ViewData["CurrentFilter"] = searchInt;
            ViewData["CurrentFilter2"] = searchString;
            var curriculum = from c in _context.Course_Curriculum
                             .Include(c => c.Course)
                             .Include(c => c.Curriculum)
                             select c;
            if (!String.IsNullOrEmpty(searchInt))
            {
                curriculum = curriculum.Where(s => s.CurriculumID.ToString() == searchInt);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                curriculum = curriculum.Where(s => s.Curriculum.Name.Contains(searchString));
            }

            return View(await curriculum.AsNoTracking().ToListAsync());
        }
        
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curriculum = await _context.Course
                .FirstOrDefaultAsync(m => m.ID == id);
            if (curriculum == null)
            {
                return NotFound();
            }

            return View(curriculum);
        }
        
        [Authorize(Roles = ApplicationRoles.AdminRole)]
        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["Course"] = _context.Course.ToList();
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseID,CurriculumID")] Course_Curriculum course)
        {
            var idplans = (from c in _context.Curriculum
                          select c.ID).ToList();

            ViewData["Course"] = _context.Course.ToList();
            if (!idplans.Contains(course.CurriculumID))
            {
                ModelState.AddModelError("CurriculumID", "Навчального плану не існує.");
            }
            else if (_context.Course_Curriculum.Contains(course))
            {
                ModelState.AddModelError("CourseID", "Даний курс вже є у вибраному навчальному плані.");
            }
            else if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        [Authorize(Roles = ApplicationRoles.AdminRole)]
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID, Name, Department_Name, Disciplines, Lectures, Practical, Laboratory, Self_Study")] Course course)
        {
            if (id != course.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.ID))
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
            return View(course);
        }

        [Authorize(Roles = ApplicationRoles.AdminRole)]
        public async Task<IActionResult> Delete(int? courseid, int? curid)
        {
            if (courseid == null)
            {
                return NotFound();
            }

            var course = _context.Course_Curriculum
                             .Include(c => c.Course)
                         .Where(c => c.CourseID == courseid)
                         .Where(c => c.CurriculumID == curid)
                         .FirstOrDefault();
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int courseid, int curriculumid)
        {
            var course = await _context.Course_Curriculum.Where(c => c.CourseID == courseid && c.CurriculumID == curriculumid)
            .FirstOrDefaultAsync();
            _context.Course_Curriculum.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.ID == id);
        }
    }
}
