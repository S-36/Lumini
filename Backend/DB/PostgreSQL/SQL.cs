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
            base.OnModelCreating(modelBuilder);
            // Add the models that are not for PostgreSQL here, for example:
            // modelBuilder.Ignore<MongoModelExample>();
        }
    }
}