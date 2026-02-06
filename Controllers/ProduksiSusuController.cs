using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimSapi.Data;
using SimSapi.Models;
using System.Security.Claims;

namespace SimSapi.Controllers
{
    [Authorize]
    public class ProduksiSusuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProduksiSusuController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var query = _context.ProduksiSusu
                .Include(p => p.Sapi)
                .AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(p => p.Sapi!.UserId == userId);
            }

            if (startDate.HasValue)
            {
                query = query.Where(p => p.Tanggal >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(p => p.Tanggal <= endDate.Value);
            }

            var produksi = await query.OrderByDescending(p => p.Tanggal).ToListAsync();

            ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd");
            ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd");

            return View(produksi);
        }

        public async Task<IActionResult> Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var sapiQuery = _context.Sapi.Where(s => s.StatusSapi == "Aktif");

            if (!isAdmin)
            {
                sapiQuery = sapiQuery.Where(s => s.UserId == userId);
            }

            ViewData["SapiId"] = new SelectList(await sapiQuery.ToListAsync(), "Id", "NamaSapi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProduksiSusu produksiSusu)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var isAdmin = User.IsInRole("Admin");

                var sapi = await _context.Sapi.FindAsync(produksiSusu.SapiId);

                if (sapi == null || (!isAdmin && sapi.UserId != userId))
                {
                    return Forbid();
                }

                _context.Add(produksiSusu);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Data produksi susu berhasil ditambahkan";
                return RedirectToAction(nameof(Index));
            }

            var userId2 = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin2 = User.IsInRole("Admin");
            var sapiQuery = _context.Sapi.Where(s => s.StatusSapi == "Aktif");

            if (!isAdmin2)
            {
                sapiQuery = sapiQuery.Where(s => s.UserId == userId2);
            }

            ViewData["SapiId"] = new SelectList(await sapiQuery.ToListAsync(), "Id", "NamaSapi", produksiSusu.SapiId);
            return View(produksiSusu);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var produksiSusu = await _context.ProduksiSusu
                .Include(p => p.Sapi)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produksiSusu == null)
            {
                return NotFound();
            }

            if (!isAdmin && produksiSusu.Sapi?.UserId != userId)
            {
                return Forbid();
            }

            var sapiQuery = _context.Sapi.Where(s => s.StatusSapi == "Aktif");

            if (!isAdmin)
            {
                sapiQuery = sapiQuery.Where(s => s.UserId == userId);
            }

            ViewData["SapiId"] = new SelectList(await sapiQuery.ToListAsync(), "Id", "NamaSapi", produksiSusu.SapiId);
            return View(produksiSusu);
        }

        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, ProduksiSusu produksiSusu)
{
    if (id != produksiSusu.Id)
    {
        return NotFound();
    }

    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var isAdmin = User.IsInRole("Admin");

    // FIX: Ambil data dengan AsNoTracking untuk validasi saja
    var existing = await _context.ProduksiSusu
        .AsNoTracking() // <-- PENTING: Jangan tracking
        .Include(p => p.Sapi)
        .FirstOrDefaultAsync(p => p.Id == id);

    if (existing == null)
    {
        return NotFound();
    }

    if (!isAdmin && existing.Sapi?.UserId != userId)
    {
        return Forbid();
    }

    if (ModelState.IsValid)
    {
        try
        {
            // FIX: Detach semua tracked entities untuk ID ini
            var trackedEntity = _context.ChangeTracker.Entries<ProduksiSusu>()
                .FirstOrDefault(e => e.Entity.Id == id);
            
            if (trackedEntity != null)
            {
                trackedEntity.State = EntityState.Detached;
            }

            _context.Update(produksiSusu);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Data produksi susu berhasil diupdate";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProduksiSusuExists(produksiSusu.Id))
            {
                return NotFound();
            }
            throw;
        }
    }

    var userId2 = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var isAdmin2 = User.IsInRole("Admin");
    var sapiQuery = _context.Sapi.Where(s => s.StatusSapi == "Aktif");

    if (!isAdmin2)
    {
        sapiQuery = sapiQuery.Where(s => s.UserId == userId2);
    }

    ViewData["SapiId"] = new SelectList(await sapiQuery.ToListAsync(), "Id", "NamaSapi", produksiSusu.SapiId);
    return View(produksiSusu);
}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var produksiSusu = await _context.ProduksiSusu
                .Include(p => p.Sapi)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produksiSusu == null)
            {
                return NotFound();
            }

            if (!isAdmin && produksiSusu.Sapi?.UserId != userId)
            {
                return Forbid();
            }

            _context.ProduksiSusu.Remove(produksiSusu);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Data produksi susu berhasil dihapus";
            return RedirectToAction(nameof(Index));
        }

        private bool ProduksiSusuExists(int id)
        {
            return _context.ProduksiSusu.Any(e => e.Id == id);
        }
    }
}