using Backend.Error;
using Backend.src.LightPoles.dtos;

namespace Backend.src.LightPoles.Interfaces
{
    public interface ILightPoleRepository
    {
        Task<LightPole?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<LightPole>> GetAllAsync(int pageNumber = 1, int pageSize = 50);
        Task<IReadOnlyList<LightPole>> GetWithinBoundsAsync(
            double latitude,
            double longitude,
            bool? isFunctional = null);
        Task<Result> AddAsync(LightPole lightPole);
        Task<Result> UpdateAsync(LightPole lightPole);
        Task<Result> DeleteAsync(Guid id);
    }

    public interface ILightPoleService
    {
        Task<Result> CreateLightPoleAsync(LightPole lightPole);
        Task<Result<IReadOnlyList<LightPoleMapMarkerDto>>> GetLightPolesMap(int pageNumber = 1, int pageSize = 50);
        Task<Result<IReadOnlyList<LightPoleMapMarkerDto>>> GetLightPolesWithinBoundsAsync(
            double latitude,
            double longitude,
            bool? isFunctional = null);
    }
}