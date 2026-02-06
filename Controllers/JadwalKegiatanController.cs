using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimSapi.Data;
using SimSapi.Models;
using System.Security.Claims;

namespace SimSapi.Controllers
{
    [Authorize]
    public class JadwalKegiatanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public JadwalKegiatanController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? status, string? jenis)
        {
            var query = _context.JadwalKegiatan
                .Include(j => j.Peserta)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(j => j.Status == status);
            }

            if (!string.IsNullOrEmpty(jenis))
            {
                query = query.Where(j => j.JenisKegiatan == jenis);
            }

            var jadwal = await query.OrderBy(j => j.TanggalMulai).ToListAsync();

            ViewData["Status"] = status;
            ViewData["Jenis"] = jenis;

            return View(jadwal);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jadwal = await _context.JadwalKegiatan
                .Include(j => j.Peserta)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jadwal == null)
            {
                return NotFound();
            }

            return View(jadwal);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(JadwalKegiatan jadwal)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                jadwal.CreatedBy = userId ?? string.Empty;
                jadwal.CreatedAt = DateTime.Now;

                _context.Add(jadwal);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Jadwal kegiatan berhasil ditambahkan";
                return RedirectToAction(nameof(Index));
            }
            return View(jadwal);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jadwal = await _context.JadwalKegiatan.FindAsync(id);

            if (jadwal == null)
            {
                return NotFound();
            }

            return View(jadwal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, JadwalKegiatan jadwal)
        {
            if (id != jadwal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingJadwal = await _context.JadwalKegiatan.FindAsync(id);

                    if (existingJadwal == null)
                    {
                        return NotFound();
                    }

                    existingJadwal.NamaKegiatan = jadwal.NamaKegiatan;
                    existingJadwal.Deskripsi = jadwal.Deskripsi;
                    existingJadwal.TanggalMulai = jadwal.TanggalMulai;
                    existingJadwal.TanggalSelesai = jadwal.TanggalSelesai;
                    existingJadwal.Lokasi = jadwal.Lokasi;
                    existingJadwal.JenisKegiatan = jadwal.JenisKegiatan;
                    existingJadwal.Status = jadwal.Status;
                    existingJadwal.PenanggungJawab = jadwal.PenanggungJawab;

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Jadwal kegiatan berhasil diupdate";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JadwalExists(jadwal.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }
            return View(jadwal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var jadwal = await _context.JadwalKegiatan.FindAsync(id);

            if (jadwal == null)
            {
                return NotFound();
            }

            _context.JadwalKegiatan.Remove(jadwal);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Jadwal kegiatan berhasil dihapus";
            return RedirectToAction(nameof(Index));
        }

        // Daftar ke kegiatan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Daftar(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Forbid();
            }

            var jadwal = await _context.JadwalKegiatan.FindAsync(id);

            if (jadwal == null)
            {
                return NotFound();
            }

            // Cek apakah sudah terdaftar
            var sudahDaftar = await _context.PesertaKegiatan
                .AnyAsync(p => p.JadwalKegiatanId == id && p.UserId == userId);

            if (sudahDaftar)
            {
                TempData["ErrorMessage"] = "Anda sudah terdaftar di kegiatan ini";
                return RedirectToAction(nameof(Details), new { id });
            }

            var peserta = new PesertaKegiatan
            {
                JadwalKegiatanId = id,
                UserId = userId
            };

            _context.PesertaKegiatan.Add(peserta);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Berhasil mendaftar ke kegiatan";
            return RedirectToAction(nameof(Details), new { id });
        }

        // Batal daftar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BatalDaftar(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Forbid();
            }

            var peserta = await _context.PesertaKegiatan
                .FirstOrDefaultAsync(p => p.JadwalKegiatanId == id && p.UserId == userId);

            if (peserta == null)
            {
                TempData["ErrorMessage"] = "Anda tidak terdaftar di kegiatan ini";
                return RedirectToAction(nameof(Details), new { id });
            }

            _context.PesertaKegiatan.Remove(peserta);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Berhasil membatalkan pendaftaran";
            return RedirectToAction(nameof(Details), new { id });
        }

        // Absen kegiatan (Admin)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Absen(int id, string userId)
        {
            var peserta = await _context.PesertaKegiatan
                .FirstOrDefaultAsync(p => p.JadwalKegiatanId == id && p.UserId == userId);

            if (peserta == null)
            {
                return NotFound();
            }

            peserta.Hadir = !peserta.Hadir;
            peserta.WaktuAbsen = peserta.Hadir ? DateTime.Now : null;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }

        private bool JadwalExists(int id)
        {
            return _context.JadwalKegiatan.Any(e => e.Id == id);
        }
    }
}