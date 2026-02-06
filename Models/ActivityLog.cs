using System.ComponentModel.DataAnnotations;

namespace SimSapi.Models
{
    public class ActivityLog
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [StringLength(50)]
        public string EntityName { get; set; } = string.Empty;

        [StringLength(10)]
        public string Action { get; set; } = string.Empty;

        public string? OldData { get; set; }

        public string? NewData { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}