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
    public class KesehatanSapiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KesehatanSapiController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? sapiId, string? jenis, string? status)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var query = _context.KesehatanSapi
                .Include(k => k.Sapi)
                .AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(k => k.Sapi!.UserId == userId);
            }

            if (sapiId.HasValue)
            {
                query = query.Where(k => k.SapiId == sapiId.Value);
            }

            if (!string.IsNullOrEmpty(jenis))
            {
                var jenisEnum = Enum.Parse<JenisPemeriksaan>(jenis);
                query = query.Where(k => k.JenisPemeriksaan == jenisEnum);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(k => k.StatusKesehatan == status);
            }

            var kesehatan = await query.OrderByDescending(k => k.TanggalPemeriksaan).ToListAsync();

            // Untuk filter dropdown
            var sapiQuery = _context.Sapi.AsQueryable();
            if (!isAdmin)
            {
                sapiQuery = sapiQuery.Where(s => s.UserId == userId);
            }
            ViewData["SapiList"] = new SelectList(await sapiQuery.ToListAsync(), "Id", "NamaSapi");

            ViewData["SapiId"] = sapiId;
            ViewData["Jenis"] = jenis;
            ViewData["Status"] = status;

            return View(kesehatan);
        }

        public async Task<IActionResult> Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var sapiQuery = _context.Sapi.Where(s => s.StatusSapi != "Mati");

            if (!isAdmin)
            {
                sapiQuery = sapiQuery.Where(s => s.UserId == userId);
            }

            ViewData["SapiId"] = new SelectList(await sapiQuery.ToListAsync(), "Id", "NamaSapi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KesehatanSapi kesehatan)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var isAdmin = User.IsInRole("Admin");

                var sapi = await _context.Sapi.FindAsync(kesehatan.SapiId);

                if (sapi == null || (!isAdmin && sapi.UserId != userId))
                {
                    return Forbid();
                }

                kesehatan.UserId = userId ?? string.Empty;
                kesehatan.CreatedAt = DateTime.Now;

                // Update status sapi jika sakit
                if (kesehatan.StatusKesehatan == "Sakit" || kesehatan.StatusKesehatan == "Dalam Perawatan")
                {
                    sapi.StatusSapi = "Sakit";
                }
                else if (kesehatan.StatusKesehatan == "Sembuh")
                {
                    sapi.StatusSapi = "Aktif";
                }

                _context.Add(kesehatan);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Data kesehatan sapi berhasil ditambahkan";
                return RedirectToAction(nameof(Index));
            }

            var userId2 = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin2 = User.IsInRole("Admin");
            var sapiQuery = _context.Sapi.Where(s => s.StatusSapi != "Mati");

            if (!isAdmin2)
            {
                sapiQuery = sapiQuery.Where(s => s.UserId == userId2);
            }

            ViewData["SapiId"] = new SelectList(await sapiQuery.ToListAsync(), "Id", "NamaSapi", kesehatan.SapiId);
            return View(kesehatan);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var kesehatan = await _context.KesehatanSapi
                .Include(k => k.Sapi)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (kesehatan == null)
            {
                return NotFound();
            }

            if (!isAdmin && kesehatan.Sapi?.UserId != userId)
            {
                return Forbid();
            }

            var sapiQuery = _context.Sapi.Where(s => s.StatusSapi != "Mati");

            if (!isAdmin)
            {
                sapiQuery = sapiQuery.Where(s => s.UserId == userId);
            }

            ViewData["SapiId"] = new SelectList(await sapiQuery.ToListAsync(), "Id", "NamaSapi", kesehatan.SapiId);
            return View(kesehatan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KesehatanSapi kesehatan)
        {
            if (id != kesehatan.Id)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingKesehatan = await _context.KesehatanSapi
                        .Include(k => k.Sapi)
                        .FirstOrDefaultAsync(k => k.Id == id);

                    if (existingKesehatan == null)
                    {
                        return NotFound();
                    }

                    if (!isAdmin && existingKesehatan.Sapi?.UserId != userId)
                    {
                        return Forbid();
                    }

                    // Update properties
                    existingKesehatan.SapiId = kesehatan.SapiId;
                    existingKesehatan.TanggalPemeriksaan = kesehatan.TanggalPemeriksaan;
                    existingKesehatan.JenisPemeriksaan = kesehatan.JenisPemeriksaan;
                    existingKesehatan.Diagnosa = kesehatan.Diagnosa;
                    existingKesehatan.Gejala = kesehatan.Gejala;
                    existingKesehatan.NamaDokter = kesehatan.NamaDokter;
                    existingKesehatan.Tindakan = kesehatan.Tindakan;
                    existingKesehatan.Obat = kesehatan.Obat;
                    existingKesehatan.BiayaPengobatan = kesehatan.BiayaPengobatan;
                    existingKesehatan.StatusKesehatan = kesehatan.StatusKesehatan;
                    existingKesehatan.TanggalKontrol = kesehatan.TanggalKontrol;
                    existingKesehatan.BeratBadan = kesehatan.BeratBadan;
                    existingKesehatan.SuhuTubuh = kesehatan.SuhuTubuh;
                    existingKesehatan.Catatan = kesehatan.Catatan;

                    // Update status sapi
                    var sapi = await _context.Sapi.FindAsync(existingKesehatan.SapiId);
                    if (sapi != null)
                    {
                        if (kesehatan.StatusKesehatan == "Sakit" || kesehatan.StatusKesehatan == "Dalam Perawatan")
                        {
                            sapi.StatusSapi = "Sakit";
                        }
                        else if (kesehatan.StatusKesehatan == "Sembuh")
                        {
                            sapi.StatusSapi = "Aktif";
                        }
                    }

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Data kesehatan sapi berhasil diupdate";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KesehatanSapiExists(kesehatan.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }

            var sapiQuery = _context.Sapi.Where(s => s.StatusSapi != "Mati");
            if (!isAdmin)
            {
                sapiQuery = sapiQuery.Where(s => s.UserId == userId);
            }

            ViewData["SapiId"] = new SelectList(await sapiQuery.ToListAsync(), "Id", "NamaSapi", kesehatan.SapiId);
            return View(kesehatan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var kesehatan = await _context.KesehatanSapi
                .Include(k => k.Sapi)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (kesehatan == null)
            {
                return NotFound();
            }

            if (!isAdmin && kesehatan.Sapi?.UserId != userId)
            {
                return Forbid();
            }

            _context.KesehatanSapi.Remove(kesehatan);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Data kesehatan sapi berhasil dihapus";
            return RedirectToAction(nameof(Index));
        }

        private bool KesehatanSapiExists(int id)
        {
            return _context.KesehatanSapi.Any(e => e.Id == id);
        }
    }
}