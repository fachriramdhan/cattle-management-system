using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimSapi.Models
{
    public class ProduksiOlahan
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NamaProduk { get; set; } = string.Empty;

        [Required]
        public JenisOlahan JenisOlahan { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,2)")]
        public decimal JumlahProduksi { get; set; } // dalam kg atau liter

        [Required]
        [StringLength(20)]
        public string Satuan { get; set; } = "Kg"; // Kg, Liter, Pcs

        [Required]
        public DateTime TanggalProduksi { get; set; } = DateTime.Now;

        public DateTime? TanggalKadaluarsa { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? BahanBakuSusu { get; set; } // Liter susu yang digunakan

        [Column(TypeName = "decimal(12,2)")]
        public decimal? BiayaProduksi { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal? HargaJual { get; set; }

        [StringLength(30)]
        public string StatusProduksi { get; set; } = "Tersedia"; // Tersedia, Terjual, Kadaluarsa

        public string? Keterangan { get; set; }

        public string UserId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public enum JenisOlahan
    {
        Yogurt = 0,
        Keju = 1,
        Susu_Pasteurisasi = 2,
        Mentega = 3,
        Es_Krim = 4,
        Kefir = 5,
        Susu_Bubuk = 6,
        Lainnya = 99
    }
}