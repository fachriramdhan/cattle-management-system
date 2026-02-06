using Microsoft.AspNetCore.Identity;

namespace SimSapi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NamaLengkap { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        
        // Relasi ke Peternak (jika user adalah peternak)
        public int? PeternakId { get; set; }
        public virtual Peternak? Peternak { get; set; }
    }
}