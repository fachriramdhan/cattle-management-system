using System.ComponentModel.DataAnnotations;

namespace SimSapi.Models
{
    public class Peternak
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string KodePeternak { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string NamaLengkap { get; set; } = string.Empty;

        [StringLength(200)]
        public string Alamat { get; set; } = string.Empty;

        [StringLength(15)]
        public string NoTelepon { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        public DateTime TanggalBergabung { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // Foreign Key ke User (jika sudah punya akun)
        public string? UserId { get; set; }

        // Navigation Properties
        public virtual ApplicationUser? User { get; set; }
        public virtual ICollection<Sapi> Sapi { get; set; } = new List<Sapi>();
    }
}