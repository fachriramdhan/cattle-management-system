namespace SimSapi.ViewModels
{
    public class DashboardViewModel
    {
        // Statistics Cards
        public int TotalSapi { get; set; }
        public int TotalPeternak { get; set; }
        public decimal ProduksiHariIni { get; set; }
        public decimal TotalPendapatan { get; set; }

        // Tren Produksi 7 Hari
        public List<TrenProduksiItem> TrenProduksi { get; set; } = new();

        // Recent Activities
        public List<RecentActivityItem> RecentActivities { get; set; } = new();
    }

    public class TrenProduksiItem
    {
        public string Tanggal { get; set; } = string.Empty;
        public decimal TotalLiter { get; set; }
        public int JumlahSapi { get; set; }
        public decimal RataRata { get; set; }
    }

    public class RecentActivityItem
    {
        public string Icon { get; set; } = string.Empty;
        public string IconColor { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
    }
}