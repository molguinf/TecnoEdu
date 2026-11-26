using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecnoEdu.Data;
using TecnoEdu.Models.Entities;
using TecnoEdu.Models.ViewModels;

namespace TecnoEdu.Controllers;

[Authorize(Policy = "InstituteAccount")]
public class InstituteManagementController : Controller
{
    private readonly TecnoEduDbContext _db;

    public InstituteManagementController(TecnoEduDbContext db)
    {
        _db = db;
    }

    private string? CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier);

    private async Task<bool> IsOwnerAsync(int instituteId)
    {
        var userId = CurrentUserId;

        return userId != null &&
            await _db.InstituteUsers.AnyAsync(x =>
                x.InstituteId == instituteId &&
                x.UserId == userId &&
                x.IsActive &&
                x.InternalRole == "Owner");
    }

    private static InstituteProfileForm ToForm(Institute institute)
    {
        return new InstituteProfileForm
        {
            Id = institute.InstituteId,
            Name = institute.Name,
            Description = institute.Description,
            City = institute.City,
            Address = institute.Address,
            Email = institute.ContactEmail,
            Phone = institute.Phone
        };
    }

    private static void ApplyForm(
        Institute institute,
        InstituteProfileForm form)
    {
        institute.Name = form.Name.Trim();
        institute.Description = form.Description.Trim();
        institute.City = form.City.Trim();
        institute.Address = form.Address.Trim();
        institute.ContactEmail = form.Email.Trim();
        institute.Phone = form.Phone.Trim();
    }

    // GET: /InstituteManagement
    public async Task<IActionResult> Index()
    {
        var userId = CurrentUserId;

        var institutes = await _db.Institutes
            .AsNoTracking()
            .Where(i => i.InstituteUsers.Any(x =>
                x.UserId == userId &&
                x.IsActive &&
                x.InternalRole == "Owner"))
            .OrderBy(i => i.Name)
            .ToListAsync();

        return View(institutes.Select(ToForm).ToList());
    }

    // GET: /InstituteManagement/Details/1
    public async Task<IActionResult> Details(int id)
    {
        var institute = await _db.Institutes
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.InstituteId == id);

        if (institute == null)
            return NotFound();

        if (!await IsOwnerAsync(id))
            return Forbid();

        return View(ToForm(institute));
    }

    // GET: /InstituteManagement/Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        if (await _db.InstituteUsers.AnyAsync(x =>
            x.UserId == CurrentUserId &&
            x.IsActive &&
            x.InternalRole == "Owner"))
        {
            return RedirectToAction(nameof(Index));
        }

        return View(new InstituteProfileForm());
    }

    // POST: /InstituteManagement/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        InstituteProfileForm form)
    {
        if (!ModelState.IsValid)
            return View(form);

        var userId = CurrentUserId;

        if (string.IsNullOrWhiteSpace(userId))
            return Challenge();

        if (await _db.InstituteUsers.AnyAsync(x =>
            x.UserId == userId &&
            x.IsActive &&
            x.InternalRole == "Owner"))
        {
            return Conflict(
                "This account already manages an institute.");
        }

        var institute = new Institute
        {
            VerificationStatus = "Pending"
        };

        ApplyForm(institute, form);

        institute.InstituteUsers.Add(new InstituteUser
        {
            UserId = userId,
            InternalRole = "Owner",
            IsActive = true
        });

        _db.Institutes.Add(institute);

        await _db.SaveChangesAsync();

        return RedirectToAction(
            nameof(Details),
            new { id = institute.InstituteId });
    }

    // GET: /InstituteManagement/Edit/1
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var institute = await _db.Institutes
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.InstituteId == id);

        if (institute == null)
            return NotFound();

        if (!await IsOwnerAsync(id))
            return Forbid();

        return View(ToForm(institute));
    }

    // POST: /InstituteManagement/Edit/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        InstituteProfileForm form)
    {
        if (id != form.Id)
            return NotFound();

        var institute = await _db.Institutes
            .FirstOrDefaultAsync(i => i.InstituteId == id);

        if (institute == null)
            return NotFound();

        if (!await IsOwnerAsync(id))
            return Forbid();

        if (!ModelState.IsValid)
            return View(form);

        ApplyForm(institute, form);

        await _db.SaveChangesAsync();

        return RedirectToAction(
            nameof(Details),
            new { id });
    }

    // GET: /InstituteManagement/Delete/1
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var institute = await _db.Institutes
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.InstituteId == id);

        if (institute == null)
            return NotFound();

        if (!await IsOwnerAsync(id))
            return Forbid();

        return View(ToForm(institute));
    }

    // POST: /InstituteManagement/Delete/1
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var institute = await _db.Institutes
            .Include(i => i.InstituteUsers)
            .FirstOrDefaultAsync(i => i.InstituteId == id);

        if (institute == null)
            return NotFound();

        if (!await IsOwnerAsync(id))
            return Forbid();

        var hasCourses = await _db.Courses
            .AnyAsync(c => c.InstituteId == id);

        var hasInquiries = await _db.Inquiries
            .AnyAsync(q => q.InstituteId == id);

        if (hasCourses || hasInquiries)
        {
            ModelState.AddModelError(
                "",
                "Cannot delete an institute with related courses or inquiries.");

            return View("Delete", ToForm(institute));
        }

        _db.InstituteUsers.RemoveRange(institute.InstituteUsers);
        _db.Institutes.Remove(institute);

        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
