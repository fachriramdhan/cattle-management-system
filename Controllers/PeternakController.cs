using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimSapi.Data;
using SimSapi.Models;

namespace SimSapi.Controllers
{
    [Authorize]
    public class PeternakController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PeternakController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var query = _context.Peternak.Include(p => p.User).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.KodePeternak.Contains(search) || 
                                        p.NamaLengkap.Contains(search) || 
                                        p.NoTelepon.Contains(search));
            }

            var peternak = await query.OrderByDescending(p => p.Id).ToListAsync();

            ViewData["Search"] = search;
            return View(peternak);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Peternak peternak)
        {
            if (ModelState.IsValid)
            {
                _context.Add(peternak);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Data peternak berhasil ditambahkan";
                return RedirectToAction(nameof(Index));
            }
            return View(peternak);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peternak = await _context.Peternak.FindAsync(id);

            if (peternak == null)
            {
                return NotFound();
            }

            return View(peternak);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Peternak peternak)
        {
            if (id != peternak.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPeternak = await _context.Peternak.FindAsync(id);

                    if (existingPeternak == null)
                    {
                        return NotFound();
                    }

                    existingPeternak.KodePeternak = peternak.KodePeternak;
                    existingPeternak.NamaLengkap = peternak.NamaLengkap;
                    existingPeternak.Alamat = peternak.Alamat;
                    existingPeternak.NoTelepon = peternak.NoTelepon;
                    existingPeternak.Email = peternak.Email;
                    existingPeternak.IsActive = peternak.IsActive;

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Data peternak berhasil diupdate";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeternakExists(peternak.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }
            return View(peternak);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var peternak = await _context.Peternak.FindAsync(id);

            if (peternak == null)
            {
                return NotFound();
            }

            _context.Peternak.Remove(peternak);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Data peternak berhasil dihapus";
            return RedirectToAction(nameof(Index));
        }

        private bool PeternakExists(int id)
        {
            return _context.Peternak.Any(e => e.Id == id);
        }
    }
}