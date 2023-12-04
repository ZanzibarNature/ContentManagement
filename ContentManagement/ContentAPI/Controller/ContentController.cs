using ContentAPI.Domain;
using ContentAPI.Domain.DTO;
using ContentAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContentAPI.Controller
{
    [Route("api/Content")]
    [ApiController]
    public class ContentController : ControllerBase, IContentController
    {
        private readonly ILocationService _locationService;
        public ContentController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpPost("Locations/Create")]
        public async Task<IActionResult> CreateLocation([FromBody] CreateLocationDTO locationDTO)
        {
            if (locationDTO == null)
            {
                return BadRequest("Invalid data in the request body");
            }

            Location newLocation = await _locationService.CreateLocation(locationDTO);

            return CreatedAtAction(nameof(GetLocationByKey), new { partitionKey = newLocation.PartitionKey, rowKey = newLocation.RowKey }, newLocation);
        }

        [HttpGet("Locations/GetByKey/{partitionKey}/{rowKey}")]
        public async Task<IActionResult> GetLocationByKey(string partitionKey, string rowKey)
        {
            var location = await _locationService.GetLocationByKeyAsync(partitionKey, rowKey);

            return location == null ? NotFound() : Ok(location);
        }
    }
}
