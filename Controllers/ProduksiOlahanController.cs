using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimSapi.Data;
using SimSapi.Models;
using System.Security.Claims;

namespace SimSapi.Controllers
{
    [Authorize]
    public class ProduksiOlahanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProduksiOlahanController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? jenis, string? status, DateTime? startDate, DateTime? endDate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var query = _context.ProduksiOlahan.AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(p => p.UserId == userId);
            }

            if (!string.IsNullOrEmpty(jenis))
            {
                var jenisEnum = Enum.Parse<JenisOlahan>(jenis);
                query = query.Where(p => p.JenisOlahan == jenisEnum);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(p => p.StatusProduksi == status);
            }

            if (startDate.HasValue)
            {
                query = query.Where(p => p.TanggalProduksi >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(p => p.TanggalProduksi <= endDate.Value);
            }

            var produksi = await query.OrderByDescending(p => p.TanggalProduksi).ToListAsync();

            ViewData["Jenis"] = jenis;
            ViewData["Status"] = status;
            ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd");
            ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd");

            return View(produksi);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProduksiOlahan produksi)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                produksi.UserId = userId ?? string.Empty;
                produksi.CreatedAt = DateTime.Now;

                _context.Add(produksi);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Data produksi olahan berhasil ditambahkan";
                return RedirectToAction(nameof(Index));
            }
            return View(produksi);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var produksi = await _context.ProduksiOlahan.FindAsync(id);

            if (produksi == null)
            {
                return NotFound();
            }

            if (!isAdmin && produksi.UserId != userId)
            {
                return Forbid();
            }

            return View(produksi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProduksiOlahan produksi)
        {
            if (id != produksi.Id)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduksi = await _context.ProduksiOlahan.FindAsync(id);

                    if (existingProduksi == null)
                    {
                        return NotFound();
                    }

                    if (!isAdmin && existingProduksi.UserId != userId)
                    {
                        return Forbid();
                    }

                    // Update properties
                    existingProduksi.NamaProduk = produksi.NamaProduk;
                    existingProduksi.JenisOlahan = produksi.JenisOlahan;
                    existingProduksi.JumlahProduksi = produksi.JumlahProduksi;
                    existingProduksi.Satuan = produksi.Satuan;
                    existingProduksi.TanggalProduksi = produksi.TanggalProduksi;
                    existingProduksi.TanggalKadaluarsa = produksi.TanggalKadaluarsa;
                    existingProduksi.BahanBakuSusu = produksi.BahanBakuSusu;
                    existingProduksi.BiayaProduksi = produksi.BiayaProduksi;
                    existingProduksi.HargaJual = produksi.HargaJual;
                    existingProduksi.StatusProduksi = produksi.StatusProduksi;
                    existingProduksi.Keterangan = produksi.Keterangan;

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Data produksi olahan berhasil diupdate";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduksiOlahanExists(produksi.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }
            return View(produksi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var produksi = await _context.ProduksiOlahan.FindAsync(id);

            if (produksi == null)
            {
                return NotFound();
            }

            if (!isAdmin && produksi.UserId != userId)
            {
                return Forbid();
            }

            _context.ProduksiOlahan.Remove(produksi);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Data produksi olahan berhasil dihapus";
            return RedirectToAction(nameof(Index));
        }

        private bool ProduksiOlahanExists(int id)
        {
            return _context.ProduksiOlahan.Any(e => e.Id == id);
        }
    }
}