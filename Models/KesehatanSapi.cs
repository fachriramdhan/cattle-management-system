using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimSapi.Models
{
    public class KesehatanSapi
    {
        public int Id { get; set; }

        [Required]
        public int SapiId { get; set; }

        [Required]
        public DateTime TanggalPemeriksaan { get; set; } = DateTime.Now;

        [Required]
        public JenisPemeriksaan JenisPemeriksaan { get; set; }

        [Required]
        [StringLength(100)]
        public string Diagnosa { get; set; } = string.Empty;

        public string Gejala { get; set; } = string.Empty;

        [StringLength(100)]
        public string? NamaDokter { get; set; }

        public string? Tindakan { get; set; }

        public string? Obat { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? BiayaPengobatan { get; set; }

        [StringLength(30)]
        public string StatusKesehatan { get; set; } = "Sehat"; // Sehat, Sakit, Dalam Perawatan, Sembuh

        public DateTime? TanggalKontrol { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? BeratBadan { get; set; } // dalam kg

        [Column(TypeName = "decimal(4,1)")]
        public decimal? SuhuTubuh { get; set; } // dalam Celsius

        public string? Catatan { get; set; }

        public string UserId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Property
        [ForeignKey("SapiId")]
        public virtual Sapi? Sapi { get; set; }
    }

    public enum JenisPemeriksaan
    {
        Pemeriksaan_Rutin = 0,
        Vaksinasi = 1,
        Pengobatan = 2,
        Pemeriksaan_Kebuntingan = 3,
        Pemeriksaan_Pasca_Melahirkan = 4,
        Pemeriksaan_Darurat = 5,
        Kastrasi = 6,
        Lainnya = 99
    }
}