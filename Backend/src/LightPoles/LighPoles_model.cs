using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.src.LightPoles
{
    [Table("LightPoles")]
    public class LightPole
    {
        [Key]
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public long latitude { get; set; } = 0;
        [Required]
        public long longitude { get; set; } = 0;
        [Required]
        public string description { get; set; } = string.Empty;
        [Required]
        public bool isFunctional { get; set; } = true;
        [Required]
        public DateTime lastMaintenance { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime installationDate { get; set; } = DateTime.UtcNow;
    }
}