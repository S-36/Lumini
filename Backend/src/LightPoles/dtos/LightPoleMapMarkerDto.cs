namespace Backend.src.LightPoles.dtos
{
    public class LightPoleMapMarkerDto
    {
        public Guid Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsFunctional { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? LuminaireType { get; set; }
    }
}
