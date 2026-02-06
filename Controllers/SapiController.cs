using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimSapi.Data;
using SimSapi.Models;
using System.Security.Claims;

namespace SimSapi.Controllers
{
    [Authorize]
    public class SapiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SapiController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search, string? jenisFilter, string? statusFilter)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var query = _context.Sapi.AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(s => s.UserId == userId);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.KodeSapi.Contains(search) || s.NamaSapi.Contains(search));
            }

            if (!string.IsNullOrEmpty(jenisFilter))
            {
                var jenis = Enum.Parse<JenisKelamin>(jenisFilter);
                query = query.Where(s => s.JenisKelamin == jenis);
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                query = query.Where(s => s.StatusSapi == statusFilter);
            }

            var sapi = await query.OrderByDescending(s => s.Id).ToListAsync();

            ViewData["Search"] = search;
            ViewData["JenisFilter"] = jenisFilter;
            ViewData["StatusFilter"] = statusFilter;

            return View(sapi);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sapi sapi)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                sapi.UserId = userId ?? string.Empty;

                _context.Add(sapi);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Data sapi berhasil ditambahkan";
                return RedirectToAction(nameof(Index));
            }
            return View(sapi);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var sapi = await _context.Sapi.FindAsync(id);

            if (sapi == null)
            {
                return NotFound();
            }

            if (!isAdmin && sapi.UserId != userId)
            {
                return Forbid();
            }

            return View(sapi);
        }

        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, Sapi sapi)
{
    if (id != sapi.Id)
    {
        return NotFound();
    }

    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var isAdmin = User.IsInRole("Admin");

    if (ModelState.IsValid)
    {
        try
        {
            // FIX: Load entity dari database, lalu update propertinya
            var existingSapi = await _context.Sapi.FindAsync(id);

            if (existingSapi == null)
            {
                return NotFound();
            }

            if (!isAdmin && existingSapi.UserId != userId)
            {
                return Forbid();
            }

            // Update properties
            existingSapi.KodeSapi = sapi.KodeSapi;
            existingSapi.NamaSapi = sapi.NamaSapi;
            existingSapi.JenisKelamin = sapi.JenisKelamin;
            existingSapi.TanggalLahir = sapi.TanggalLahir;
            existingSapi.StatusSapi = sapi.StatusSapi;

            // Tidak perlu Update() karena entity sudah tracked
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Data sapi berhasil diupdate";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SapiExists(sapi.Id))
            {
                return NotFound();
            }
            throw;
        }
    }
    return View(sapi);
}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var sapi = await _context.Sapi.FindAsync(id);

            if (sapi == null)
            {
                return NotFound();
            }

            if (!isAdmin && sapi.UserId != userId)
            {
                return Forbid();
            }

            _context.Sapi.Remove(sapi);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Data sapi berhasil dihapus";
            return RedirectToAction(nameof(Index));
        }

        private bool SapiExists(int id)
        {
            return _context.Sapi.Any(e => e.Id == id);
        }
    }
}