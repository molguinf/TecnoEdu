using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecnoEdu.Data;
using TecnoEdu.Models;

namespace TecnoEdu.Controllers
{
    [Authorize(Policy = "InstituteAccount")]
    public class InstitutesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InstitutesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var institutes = _context.Institutes.Where(institute => institute.OwnerId == ownerId);
            return View(await institutes.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var institute = await _context.Institutes.AsNoTracking().FirstOrDefaultAsync(institute => institute.Id == id);
            if (institute == null)
            {
                return NotFound();
            }

            if (institute.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            return View(institute);
        }

        public async Task<IActionResult> Create()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (await _context.Institutes.AnyAsync(institute => institute.OwnerId == ownerId))
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,City,Address,Email,Phone")] Institute institute)
        {
            institute.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (await _context.Institutes.AnyAsync(item => item.OwnerId == institute.OwnerId))
            {
                return Conflict("An institute profile already exists for this account.");
            }

            if (ModelState.IsValid)
            {
                _context.Institutes.Add(institute);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (await _context.Institutes.AnyAsync(item => item.OwnerId == institute.OwnerId))
                    {
                        return Conflict("An institute profile already exists for this account.");
                    }

                    throw;
                }

                return RedirectToAction(nameof(Details), new { id = institute.Id });
            }

            return View(institute);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var institute = await _context.Institutes.FindAsync(id);
            if (institute == null)
            {
                return NotFound();
            }

            if (institute.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            return View(institute);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, [Bind("Id,Name,Description,City,Address,Email,Phone")] Institute institute)
        {
            var currentInstitute = await _context.Institutes.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);
            if (currentInstitute == null)
            {
                return NotFound();
            }

            if (currentInstitute.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            if (id != institute.Id)
            {
                return NotFound();
            }

            institute.OwnerId = currentInstitute.OwnerId;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Institutes.Update(institute);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Institutes.AnyAsync(item => item.Id == id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Details), new { id = institute.Id });
            }

            return View(institute);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var institute = await _context.Institutes.AsNoTracking().FirstOrDefaultAsync(institute => institute.Id == id);
            if (institute == null)
            {
                return NotFound();
            }

            if (institute.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            return View(institute);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var institute = await _context.Institutes.FindAsync(id);
            if (institute == null)
            {
                return NotFound();
            }

            if (institute.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            _context.Institutes.Remove(institute);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
