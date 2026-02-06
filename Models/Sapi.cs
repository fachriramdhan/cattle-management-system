using System.ComponentModel.DataAnnotations;

namespace SimSapi.Models
{
    public class Sapi
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string KodeSapi { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string NamaSapi { get; set; } = string.Empty;

        public JenisKelamin JenisKelamin { get; set; }

        [Required]
        public DateTime TanggalLahir { get; set; }

        [StringLength(30)]
        public string StatusSapi { get; set; } = "Aktif";

        public string UserId { get; set; } = string.Empty;

        // Navigation Property
        public virtual ICollection<ProduksiSusu> ProduksiSusu { get; set; } = new List<ProduksiSusu>();
    }

    public enum JenisKelamin
    {
        Jantan = 0,
        Betina = 1
    }
}