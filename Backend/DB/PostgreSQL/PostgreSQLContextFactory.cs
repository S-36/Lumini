using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Backend.DB.PostgreSQL
{
    public class PostgreSQLContextFactory : IDesignTimeDbContextFactory<PostgreSQLContext>
    {
        public PostgreSQLContext CreateDbContext(string[] args)
        {
            // Load environment variables from .env file
            Env.Load();

            var connectionString = Environment.GetEnvironmentVariable("POSTGRESQL_CONECTION")
                ?? throw new InvalidOperationException("POSTGRESQL_CONECTION environment variable is not set");

            var optionsBuilder = new DbContextOptionsBuilder<PostgreSQLContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new PostgreSQLContext(optionsBuilder.Options);
        }
    }
}
