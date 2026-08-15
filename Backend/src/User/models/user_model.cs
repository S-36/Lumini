using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.src.User
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid();
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;
        [MaxLength(300)]
        public string PasswordHash { get; set; } = string.Empty;
        [MaxLength(50)]
        [AllowedValues("Admin", "Manager", "Worker")]
        public List<string> UserRoles { get; set; } = [];
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // additional or optional fields 
        public string? Address { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}