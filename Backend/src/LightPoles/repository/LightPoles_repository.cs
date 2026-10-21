using Backend.DB.PostgreSQL;
using Backend.Error;
using Backend.src.LightPoles.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.src.LightPoles.repository
{
    public class LightPoleRepository : ILightPoleRepository
    {
        private readonly PostgreSQLContext _context;

        public LightPoleRepository(PostgreSQLContext context)
        {
            _context = context;
        }

        public async Task<LightPole?> GetByIdAsync(Guid id)
        {
            return await _context.LightPoles.FindAsync(id);
        }

        public async Task<IReadOnlyList<LightPole>> GetAllAsync(int pageNumber = 1, int pageSize = 50)
        {
            return await _context.LightPoles
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<LightPole>> GetWithinBoundsAsync(
            double latitude,
            double longitude,
            bool? isFunctional = null)
        {
            var query = _context.LightPoles.AsQueryable();

            if (isFunctional.HasValue)
            {
                query = query.Where(lp => lp.IsFunctional == isFunctional.Value);
            }

            // Assuming a simple bounding box for demonstration purposes
            double latRange = 0.1; // Adjust as needed
            double lonRange = 0.1; // Adjust as needed

            query = query.Where(lp =>
                lp.Latitude >= latitude - latRange && lp.Latitude <= latitude + latRange &&
                lp.Longitude >= longitude - lonRange && lp.Longitude <= longitude + lonRange);

            return await query.ToListAsync();
        }

        public async Task<Result> AddAsync(LightPole lightPole)
        {
            _context.LightPoles.Add(lightPole);
            await _context.SaveChangesAsync();
            return Result.Success(201);
        }

        public async Task<Result> UpdateAsync(LightPole lightPole)
        {
            _context.LightPoles.Update(lightPole);
            await _context.SaveChangesAsync();
            return Result.Success(200);
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var lightPole = await _context.LightPoles.FindAsync(id);
            if (lightPole == null) return Result.Failure("Light pole not found", 404);

            _context.LightPoles.Remove(lightPole);
            await _context.SaveChangesAsync();
            return Result.Success(200);
        }
    }
}