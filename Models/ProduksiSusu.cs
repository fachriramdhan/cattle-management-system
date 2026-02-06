using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimSapi.Models
{
    public class ProduksiSusu
    {
        public int Id { get; set; }

        [Required]
        public int SapiId { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal VolumeLiter { get; set; }

        [Required]
        public WaktuPerah WaktuPerah { get; set; }

        [Required]
        public DateTime Tanggal { get; set; } = DateTime.Now;

        // Navigation Property
        [ForeignKey("SapiId")]
        public virtual Sapi? Sapi { get; set; }
    }

    public enum WaktuPerah
    {
        Pagi = 0,
        Sore = 1
    }
}