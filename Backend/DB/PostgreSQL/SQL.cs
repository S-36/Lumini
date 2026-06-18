using Backend.src.LightPoles;
using Backend.src.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Backend.DB.PostgreSQL
{
    public class SQLSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }

    public interface ISQLContext
    {
        DbSet<T> GetDbSet<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void Dispose();
    }

    public class PostgreSQLContext : DbContext, ISQLContext
    {
        public PostgreSQLContext(DbContextOptions<PostgreSQLContext> options) : base(options) {}
        
            // Add and set the models for the PostgreSQL database here, for example:
            // public DbSet<YourEntity> YourEntities { get; set; } = null;
            public DbSet<User> Users { get; set; } = null!;
            public DbSet<LightPole> LightPoles { get; set; } = null!;
        

        public DbSet<T> GetDbSet<T>() where T : class
        {
            return Set<T>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Datetime configuration for PostgreSQL
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", false);
            // User Model Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(300);
                entity.Property(e => e.UserRoles).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
            });
            
            modelBuilder.Entity<LightPole>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Latitude).IsRequired();
                entity.Property(e => e.Longitude).IsRequired();
                entity.Property(e => e.Description).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(200);
                entity.Property(e => e.City).IsRequired().HasMaxLength(100);
                entity.Property(e => e.District).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LuminaireType).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CreatedByUserId).IsRequired();
            });

            // Add the models that are not for PostgreSQL here, for example:
            // modelBuilder.Ignore<MongoModelExample>();
        }
    }
}