using System.ComponentModel.DataAnnotations;

namespace SimSapi.Models
{
    public class JadwalKegiatan
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string NamaKegiatan { get; set; } = string.Empty;

        [Required]
        public string Deskripsi { get; set; } = string.Empty;

        [Required]
        public DateTime TanggalMulai { get; set; }

        [Required]
        public DateTime TanggalSelesai { get; set; }

        [StringLength(50)]
        public string Lokasi { get; set; } = string.Empty;

        [StringLength(50)]
        public string JenisKegiatan { get; set; } = "Umum"; // Umum, Vaksinasi, Pelatihan, dll

        [StringLength(30)]
        public string Status { get; set; } = "Terjadwal"; // Terjadwal, Berlangsung, Selesai, Dibatalkan

        public string? PenanggungJawab { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Property
        public virtual ICollection<PesertaKegiatan> Peserta { get; set; } = new List<PesertaKegiatan>();
    }

    // Tabel relasi Many-to-Many untuk peserta kegiatan
    public class PesertaKegiatan
    {
        public int Id { get; set; }

        public int JadwalKegiatanId { get; set; }
        public string UserId { get; set; } = string.Empty;

        public bool Hadir { get; set; } = false;
        public DateTime? WaktuAbsen { get; set; }

        // Navigation Properties
        public virtual JadwalKegiatan? JadwalKegiatan { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}