using Backend.Error;
using Backend.src.LightPoles.dtos;
using Backend.src.LightPoles.Interfaces;

namespace Backend.src.LightPoles.services 
{
    public class LightPoleService : ILightPoleService
    {
        private readonly ILightPoleRepository _lightPoleRepository;

        public LightPoleService(ILightPoleRepository lightPoleRepository)
        {
            _lightPoleRepository = lightPoleRepository;
        }

        public async Task<Result> CreateLightPoleAsync(LightPole lightPole)
        {
            return await _lightPoleRepository.AddAsync(lightPole);
        }

        public async Task<Result<IReadOnlyList<LightPoleMapMarkerDto>>> GetLightPolesMap(int pageNumber = 1, int pageSize = 50)
        {
            var lightPoles = await _lightPoleRepository.GetAllAsync(pageNumber, pageSize);
            var lightPolesDto = lightPoles.Select(lp => new LightPoleMapMarkerDto
            {
                Id = lp.Id,
                Latitude = lp.Latitude,
                Longitude = lp.Longitude,
                IsFunctional = lp.IsFunctional
            }).ToList();

            return Result<IReadOnlyList<LightPoleMapMarkerDto>>.Success(lightPolesDto, 200);
        }

        public async Task<Result<IReadOnlyList<LightPoleMapMarkerDto>>> GetLightPolesWithinBoundsAsync(
            double latitude,
            double longitude,
            bool? isFunctional = null)
        {
            var lightPoles = await _lightPoleRepository.GetWithinBoundsAsync(latitude, longitude, isFunctional);
            var lightPolesDto = lightPoles.Select(lp => new LightPoleMapMarkerDto
            {
                Id = lp.Id,
                Latitude = lp.Latitude,
                Longitude = lp.Longitude,
                IsFunctional = lp.IsFunctional
            }).ToList();

            return Result<IReadOnlyList<LightPoleMapMarkerDto>>.Success(lightPolesDto, 200);
        }
    }
}