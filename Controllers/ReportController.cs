using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimSapi.Data;
using SimSapi.Services;
using System.Security.Claims;

namespace SimSapi.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ReportService _reportService;

        public ReportController(ApplicationDbContext context, ReportService reportService)
        {
            _context = context;
            _reportService = reportService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF(DateTime startDate, DateTime endDate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var query = _context.ProduksiSusu
                .Include(p => p.Sapi)
                .Where(p => p.Tanggal >= startDate && p.Tanggal <= endDate);

            if (!isAdmin)
            {
                query = query.Where(p => p.Sapi!.UserId == userId);
            }

            var data = await query.OrderBy(p => p.Tanggal).ToListAsync();

            if (!data.Any())
            {
                TempData["ErrorMessage"] = "Tidak ada data untuk periode yang dipilih";
                return RedirectToAction(nameof(Index));
            }

            var pdfBytes = _reportService.GenerateProduksiReport(data, startDate, endDate);

            return File(pdfBytes, "application/pdf", $"Laporan_Produksi_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.pdf");
        }
    }
}