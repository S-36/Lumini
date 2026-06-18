using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.src.LightPoles
{
    [Table("LightPoles")]
    public class LightPole
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Range(-90.0, 90.0)]
        public double Latitude { get; set; } 
        [Range(-180.0, 180.0)]
        public double Longitude { get; set; } 
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? Address { get; set; }
        [MaxLength(100)]
        public string? City { get; set; }
        [MaxLength(100)]
        public string? District { get; set; }
        public bool IsFunctional { get; set; } = true;
        public int? WattageRating { get; set; } // power in watts
        [MaxLength(100)]
        public string? LuminaireType { get; set; } // Type of luminaire (LED, HPS, etc.)
        public DateTime InstallationDate { get; set; }
        public DateTime? LastMaintenanceAt { get; set; }
        public DateTime? NextMaintenanceAt { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public Guid CreatedByUserId { get; set; }
    }
}