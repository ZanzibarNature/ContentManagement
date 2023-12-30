using Azure;
using ContentAPI.Controllers.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Domain.DTO;
using ContentAPI.Domain.RequestModel;
using ContentAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContentAPI.Controllers
{
    [Route("api/Content/Locations")]
    [ApiController]
    public class LocationController : ControllerBase, ILocationController
    {
        private readonly ILocationService _locationService;
        private readonly IBlobStorageService _blobService;
        public LocationController(ILocationService locationService, IBlobStorageService blobService)
        {
            _locationService = locationService;
            _blobService = blobService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateLocation([FromBody] CreateLocationDTO locationDTO)
        {
            if (locationDTO == null)
            {
                return BadRequest("LocationDTO object is null or invalid");
            }

            Location newLocation = await _locationService.CreateLocation(locationDTO);

            return CreatedAtAction(nameof(GetLocationByKey), new { partitionKey = newLocation.PartitionKey, rowKey = newLocation.RowKey }, newLocation);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateLocation([FromBody] LocationUpdateRequestModel model)
        {
            if (model.DTO == null || model.OldLocation == null)
            {
                return BadRequest("One or more of the given objects are null or invalid");
            }

            Location updatedLocation = await _locationService.UpdateLocationAsync(model.DTO, model.OldLocation);

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
            Response response = await _locationService.DeleteLocationAsync(partitionKey, rowKey);

            return response.IsError ? NotFound("Given keypair does not exist") : NoContent();
        }
    }
}
