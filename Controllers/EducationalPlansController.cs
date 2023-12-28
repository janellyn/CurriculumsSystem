using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol.Core.Types;
using TaskAuthenticationAuthorization.Models;

namespace TaskAuthenticationAuthorization.Controllers
{
    public class EducationalPlansController : Controller
    {
        private readonly EducationalPlansContext _context;

        public EducationalPlansController(EducationalPlansContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["IDSortParam"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "Name";
            ViewData["Education_LevelSortParam"] = sortOrder == "Education_Level" ? "Education_Level_desc" : "Education_Level";
            ViewData["Learning_FormSortParam"] = sortOrder == "Learning_Form" ? "Learning_Form_desc" : "Learning_Form";
            ViewData["Faculty_InstituteSortParam"] = sortOrder == "Faculty_Institute" ? "Faculty_Institute_desc" : "Faculty_Institute";
            ViewData["SpecialitySortParam"] = sortOrder == "Speciality" ? "speciality_desc" : "Speciality";
            ViewData["Educational_ProgramSortParam"] = sortOrder == "Educational_Program" ? "Educational_Program_desc" : "Educational_Program";
            ViewData["CurrentFilter"] = searchString;
            var curriculum = from c in _context.Curriculum
                             .Include(c => c.Education_Level)
                             .Include(c => c.Learning_Form)
                             .Include(c => c.Faculty_Institute)
                             .Include(c => c.Speciality)
                             .Include(c => c.Educational_Program)
                             select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                curriculum = curriculum.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "id_desc":
                    curriculum = curriculum.OrderByDescending(s => s.ID);
                    break;
                case "Name":
                    curriculum = curriculum.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    curriculum = curriculum.OrderByDescending(s => s.Name);
                    break;
                case "Speciality":
                    curriculum = curriculum.OrderBy(s => s.Speciality.Name);
                    break;
                case "speciality_desc":
                    curriculum = curriculum.OrderByDescending(s => s.Speciality.Name);
                    break;
                case "Education_Level":
                    curriculum = curriculum.OrderBy(s => s.Education_Level.Name);
                    break;
                case "Education_Level_desc":
                    curriculum = curriculum.OrderByDescending(s => s.Education_Level.Name);
                    break;
                case "Learning_Form":
                    curriculum = curriculum.OrderBy(s => s.Learning_Form.Name);
                    break;
                case "Learning_Form_desc":
                    curriculum = curriculum.OrderByDescending(s => s.Learning_Form.Name);
                    break;
                case "Faculty_Institute":
                    curriculum = curriculum.OrderBy(s => s.Faculty_Institute.Name);
                    break;
                case "Faculty_Institute_desc":
                    curriculum = curriculum.OrderByDescending(s => s.Faculty_Institute.Name);
                    break;
                case "Educational_Program":
                    curriculum = curriculum.OrderBy(s => s.Educational_Program.Name);
                    break;
                case "Educational_Program_desc":
                    curriculum = curriculum.OrderByDescending(s => s.Educational_Program.Name);
                    break;
                default:
                    curriculum = curriculum.OrderBy(s => s.ID);
                    break;

            }

            return View(await curriculum.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curriculum = await _context.Curriculum
                             .Include(c => c.Education_Level)
                             .Include(c => c.Learning_Form)
                             .Include(c => c.Faculty_Institute)
                             .Include(c => c.Speciality)
                             .Include(c => c.Educational_Program)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (curriculum == null)
            {
                return NotFound();
            }

            return View(curriculum);
        }

        [Authorize(Roles = ApplicationRoles.AdminRole)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.AdminRole)]
        public async Task<IActionResult> Create([Bind("ID,Name,Education_LevelId,Learning_FormId,Faculty_InstituteId,SpecialityId,Educational_ProgramId")] Curriculum curriculum)
        {
            var spec = (from c in _context.Speciality
                             where c.Faculty_InstituteId == curriculum.Faculty_InstituteId
                             select c.ID).ToList();

            var educprog = (from c in _context.Educational_Program
                            where c.SpecialityId == curriculum.SpecialityId
                            select c.ID).ToList();
            var ids = (from c in _context.Curriculum
                       select c.ID).ToList();

            if (ids.Contains(curriculum.ID))
            {
                ModelState.AddModelError("ID", "Номер вже існує.");
            }
            else if (!spec.Contains(curriculum.SpecialityId))
            {
                ModelState.AddModelError("SpecialityId", "Даної спеціальності немає у вибраному факультеті.");
            }
            else if (!educprog.Contains(curriculum.Educational_ProgramId))
            {
                ModelState.AddModelError("Educational_ProgramId", "Даної освітньої програми немає у вибраній спеціальності.");
            }
            else if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Educational_ProgramId", "Дані не валідні.");
            }
            else if (ModelState.IsValid)
            {
                _context.Add(curriculum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(curriculum);
        }


        [Authorize(Roles = ApplicationRoles.UserRole)]
        public async Task<IActionResult> AddToCabinet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curriculum = await _context.Curriculum
                             .Include(c => c.Education_Level)
                             .Include(c => c.Learning_Form)
                             .Include(c => c.Faculty_Institute)
                             .Include(c => c.Speciality)
                             .Include(c => c.Educational_Program)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (curriculum == null)
            {
                return NotFound();
            }
            return View(curriculum);
        }

        [HttpPost, ActionName("AddToCabinet")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.UserRole)]
        public async Task<IActionResult> AddToCabinetConfirmed(int? id)
        {
                var cabid = HttpContext.Session.GetInt32("CabinetId");
                Cabinet_Curriculum cab = new Cabinet_Curriculum() { CabinetID = (int)cabid, CurriculumID = (int)id };
                if (_context.Cabinet_Curriculum.Contains(cab))
                {
                    ModelState.AddModelError("ID", "План вже є у кабінеті.");
                } else
                _context.Add(cab);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = ApplicationRoles.AdminRole)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curriculum = await _context.Curriculum.FindAsync(id);
            if (curriculum == null)
            {
                return NotFound();
            }
            return View(curriculum);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.AdminRole)]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Education_LevelId,Learning_FormId,Faculty_InstituteId,SpecialityId,Educational_ProgramId")] Curriculum curriculum)
        {
            var spec = (from c in _context.Speciality
                        where c.Faculty_InstituteId == curriculum.Faculty_InstituteId
                        select c.ID).ToList();

            var educprog = (from c in _context.Educational_Program
                            where c.SpecialityId == curriculum.SpecialityId
                            select c.ID).ToList();

            if (id != curriculum.ID)
            {
                return NotFound();
            }

            if (!spec.Contains(curriculum.SpecialityId))
            {
                ModelState.AddModelError("SpecialityId", "Даної спеціальності немає у вибраному факультеті.");
            }
            else if (!educprog.Contains(curriculum.Educational_ProgramId))
            {
                ModelState.AddModelError("Educational_ProgramId", "Даної освітньої програми немає у вибраній спеціальності.");
            }
            else if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Educational_ProgramId", "Дані не валідні.");
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(curriculum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CurriculumExists(curriculum.ID))
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
            return View(curriculum);
        }

        [Authorize(Roles = ApplicationRoles.AdminRole)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curriculum = await _context.Curriculum
                             .Include(c => c.Education_Level)
                             .Include(c => c.Learning_Form)
                             .Include(c => c.Faculty_Institute)
                             .Include(c => c.Speciality)
                             .Include(c => c.Educational_Program)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (curriculum == null)
            {
                return NotFound();
            }

            return View(curriculum);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.AdminRole)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var curriculum = await _context.Curriculum.FindAsync(id);
            _context.Curriculum.Remove(curriculum);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CurriculumExists(int id)
        {
            return _context.Curriculum.Any(e => e.ID == id);
        }
    }
}

