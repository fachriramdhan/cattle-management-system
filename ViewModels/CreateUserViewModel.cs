using System.ComponentModel.DataAnnotations;

namespace SimSapi.ViewModels
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Nama Lengkap harus diisi")]
        [StringLength(100)]
        public string NamaLengkap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email harus diisi")]
        [EmailAddress(ErrorMessage = "Format email tidak valid")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password harus diisi")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password minimal 6 karakter")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role harus dipilih")]
        public string Role { get; set; } = "User";

        public int? PeternakId { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nama Lengkap harus diisi")]
        [StringLength(100)]
        public string NamaLengkap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email harus diisi")]
        [EmailAddress(ErrorMessage = "Format email tidak valid")]
        public string Email { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password minimal 6 karakter")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Role harus dipilih")]
        public string Role { get; set; } = "User";

        public bool IsActive { get; set; }

        public int? PeternakId { get; set; }
    }
}