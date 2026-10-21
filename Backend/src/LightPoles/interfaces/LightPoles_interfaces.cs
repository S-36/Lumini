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
        Task<LightPole> AddAsync(LightPole lightPole);
        Task<bool> UpdateAsync(LightPole lightPole);
        Task<bool> DeleteAsync(Guid id);
    }

    public interface ILightPoleService
    {
        
    }
}