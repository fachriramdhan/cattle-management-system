using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimSapi.Data;
using SimSapi.ViewModels;
using System.Globalization;

namespace SimSapi.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel();

            // ==================== STATISTICS CARDS ====================
            
            // 1. Total Sapi
            viewModel.TotalSapi = await _context.Sapi.CountAsync();

            // 2. Total Peternak
            viewModel.TotalPeternak = await _context.Peternak.CountAsync();

            // 3. Produksi Hari Ini
            var today = DateTime.Today;
            viewModel.ProduksiHariIni = await _context.ProduksiSusu
                .Where(p => p.Tanggal.Date == today)
                .SumAsync(p => p.VolumeLiter);

            // 4. Total Pendapatan Bulan Ini (dari Produksi Olahan)
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            viewModel.TotalPendapatan = await _context.ProduksiOlahan
                .Where(p => p.TanggalProduksi >= firstDayOfMonth)
                .SumAsync(p => p.HargaJual ?? 0);

            // ==================== TREN PRODUKSI 7 HARI ====================
            
            var sevenDaysAgo = today.AddDays(-6); // 7 hari termasuk hari ini
            
            var produksiData = await _context.ProduksiSusu
                .Where(p => p.Tanggal.Date >= sevenDaysAgo && p.Tanggal.Date <= today)
                .GroupBy(p => p.Tanggal.Date)
                .Select(g => new
                {
                    Tanggal = g.Key,
                    TotalLiter = g.Sum(p => p.VolumeLiter),
                    JumlahSapi = g.Select(p => p.SapiId).Distinct().Count()
                })
                .OrderBy(x => x.Tanggal)
                .ToListAsync();

            // Buat list 7 hari (isi 0 jika tidak ada data)
            var culture = new CultureInfo("id-ID");
            for (int i = 0; i < 7; i++)
            {
                var date = sevenDaysAgo.AddDays(i);
                var data = produksiData.FirstOrDefault(p => p.Tanggal == date);

                viewModel.TrenProduksi.Add(new TrenProduksiItem
                {
                    Tanggal = date.ToString("dd MMM", culture),
                    TotalLiter = data?.TotalLiter ?? 0,
                    JumlahSapi = data?.JumlahSapi ?? 0,
                    RataRata = data != null && data.JumlahSapi > 0 
                        ? Math.Round(data.TotalLiter / data.JumlahSapi, 2) 
                        : 0
                });
            }

            // ==================== RECENT ACTIVITIES ====================
            
            // 1. Sapi Baru Ditambahkan
            var recentSapi = await _context.Sapi
                .OrderByDescending(s => s.Id)
                .Take(1)
                .FirstOrDefaultAsync();

            if (recentSapi != null)
            {
                viewModel.RecentActivities.Add(new RecentActivityItem
                {
                    Icon = "fa-cow",
                    IconColor = "bg-blue-100 text-blue-600",
                    Title = "Sapi Baru Ditambahkan",
                    Description = $"{recentSapi.NamaSapi} ({recentSapi.KodeSapi})",
                    Time = GetRelativeTime(recentSapi.TanggalLahir)
                });
            }

            // 2. Produksi Susu Tercatat
            var recentProduksiSusu = await _context.ProduksiSusu
                .Include(p => p.Sapi)
                .OrderByDescending(p => p.Tanggal)
                .Take(1)
                .FirstOrDefaultAsync();

            if (recentProduksiSusu != null)
            {
                viewModel.RecentActivities.Add(new RecentActivityItem
                {
                    Icon = "fa-droplet",
                    IconColor = "bg-green-100 text-green-600",
                    Title = "Produksi Susu Tercatat",
                    Description = $"{recentProduksiSusu.VolumeLiter} liter - {recentProduksiSusu.Sapi?.NamaSapi}",
                    Time = GetRelativeTime(recentProduksiSusu.Tanggal)
                });
            }

            // 3. Produksi Olahan Terbaru
            var recentOlahan = await _context.ProduksiOlahan
                .OrderByDescending(p => p.TanggalProduksi)
                .Take(1)
                .FirstOrDefaultAsync();

            if (recentOlahan != null)
            {
                viewModel.RecentActivities.Add(new RecentActivityItem
                {
                    Icon = "fa-cheese",
                    IconColor = "bg-yellow-100 text-yellow-600",
                    Title = "Produksi Olahan",
                    Description = $"{recentOlahan.NamaProduk} - {recentOlahan.JumlahProduksi} {recentOlahan.Satuan}",
                    Time = GetRelativeTime(recentOlahan.TanggalProduksi)
                });
            }

            // 4. Kesehatan Sapi Terbaru
            var recentKesehatan = await _context.KesehatanSapi
                .Include(k => k.Sapi)
                .OrderByDescending(k => k.TanggalPemeriksaan)
                .Take(1)
                .FirstOrDefaultAsync();

            if (recentKesehatan != null)
            {
                viewModel.RecentActivities.Add(new RecentActivityItem
                {
                    Icon = "fa-heartbeat",
                    IconColor = "bg-red-100 text-red-600",
                    Title = "Pemeriksaan Kesehatan",
                    Description = $"{recentKesehatan.Sapi?.NamaSapi} - {recentKesehatan.JenisPemeriksaan}",
                    Time = GetRelativeTime(recentKesehatan.TanggalPemeriksaan)
                });
            }

            // 5. Peternak Baru Bergabung
            var recentPeternak = await _context.Peternak
                .OrderByDescending(p => p.TanggalBergabung)
                .Take(1)
                .FirstOrDefaultAsync();

            if (recentPeternak != null)
            {
                viewModel.RecentActivities.Add(new RecentActivityItem
                {
                    Icon = "fa-user-plus",
                    IconColor = "bg-purple-100 text-purple-600",
                    Title = "Peternak Baru Bergabung",
                    Description = $"{recentPeternak.NamaLengkap} ({recentPeternak.KodePeternak})",
                    Time = GetRelativeTime(recentPeternak.TanggalBergabung)
                });
            }

            // Batasi maksimal 5 aktivitas terbaru (jika ada lebih dari 5)
            viewModel.RecentActivities = viewModel.RecentActivities
                .Take(5)
                .ToList();

            return View(viewModel);
        }

        /// <summary>
        /// Helper method untuk menghitung waktu relatif
        /// </summary>
        private string GetRelativeTime(DateTime date)
        {
            var timeSpan = DateTime.Now - date;
            
            if (timeSpan.TotalDays >= 365)
                return $"{(int)(timeSpan.TotalDays / 365)} tahun yang lalu";
            if (timeSpan.TotalDays >= 30)
                return $"{(int)(timeSpan.TotalDays / 30)} bulan yang lalu";
            if (timeSpan.TotalDays >= 7)
                return $"{(int)(timeSpan.TotalDays / 7)} minggu yang lalu";
            if (timeSpan.TotalDays >= 1)
                return $"{(int)timeSpan.TotalDays} hari yang lalu";
            if (timeSpan.TotalHours >= 1)
                return $"{(int)timeSpan.TotalHours} jam yang lalu";
            if (timeSpan.TotalMinutes >= 1)
                return $"{(int)timeSpan.TotalMinutes} menit yang lalu";
            
            return "Baru saja";
        }
    }
}