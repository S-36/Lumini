using Backend.src.LightPoles.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.src.LightPoles.controller
{
    public class LightPoleController : ControllerBase
    {
        private readonly ILightPoleService _lightPoleService;

        public LightPoleController(ILightPoleService lightPoleService)
        {
            _lightPoleService = lightPoleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLightPole([FromBody] LightPole lightPole)
        {
            var result = await _lightPoleService.CreateLightPoleAsync(lightPole);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }
            return StatusCode(result.StatusCode);
        }

        [HttpGet("map")]
        public async Task<IActionResult> GetLightPolesMap(int pageNumber = 1, int pageSize = 50)
        {
            var result = await _lightPoleService.GetLightPolesMap(pageNumber, pageSize);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }
            return Ok(result);
        }

        [HttpGet("bounds")]
        public async Task<IActionResult> GetLightPolesWithinBounds(
            double latitude,
            double longitude,
            bool? isFunctional = null)
        {
            var result = await _lightPoleService.GetLightPolesWithinBoundsAsync(latitude, longitude, isFunctional);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }
            return Ok(result);
        }
    }
}