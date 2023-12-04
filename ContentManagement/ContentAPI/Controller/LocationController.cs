using ContentAPI.Controller.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Domain.DTO;
using ContentAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContentAPI.Controller
{
    [Route("api/Content/Locations")]
    [ApiController]
    public class LocationController : ControllerBase, ILocationController
    {
        private readonly ILocationService _locationService;
        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateLocation([FromBody] CreateLocationDTO locationDTO)
        {
            if (locationDTO == null)
            {
                return BadRequest("Invalid data in the request body");
            }

            Location newLocation = await _locationService.CreateLocation(locationDTO);

            return CreatedAtAction(nameof(GetLocationByKey), new { partitionKey = newLocation.PartitionKey, rowKey = newLocation.RowKey }, newLocation);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateLocation([FromBody] Location location)
        {
            if (location == null)
            {
                return BadRequest("Invalid data in the request body");
            }

            Location updatedLocation = await _locationService.UpdateLocationAsync(location);

            return Ok(updatedLocation);
        }

        [HttpGet("GetByKey/{partitionKey}/{rowKey}")]
        public async Task<IActionResult> GetLocationByKey(string partitionKey, string rowKey)
        {
            var location = await _locationService.GetLocationByKeyAsync(partitionKey, rowKey);

            return location == null ? NotFound() : Ok(location);
        }

        [HttpDelete("Delete/{partitionKey}/{rowKey}")]
        public async Task<IActionResult> DeleteLocation(string partitionKey, string rowKey)
        {
            await _locationService.DeleteLocationAsync(partitionKey, rowKey);

            return NoContent();
        }
    }
}
