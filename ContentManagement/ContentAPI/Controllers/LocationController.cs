using Azure;
using ContentAPI.Controllers.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Domain.DTO;
using ContentAPI.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> Create([FromBody] CreateLocationDTO DTO)
        {
            if (DTO == null)
            {
                return BadRequest("LocationDTO object is null or invalid");
            }

            Location newLocation = await _locationService.Create(DTO);

            return CreatedAtAction(nameof(GetByKey), new { partitionKey = newLocation.PartitionKey, rowKey = newLocation.RowKey }, newLocation);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateLocationDTO DTO)
        {
            if (DTO == null)
            {
                return BadRequest("The given DTO is null or invalid");
            }

            Location updatedLocation = await _locationService.UpdateAsync(DTO);

            return Ok(updatedLocation);
        }

        [HttpGet("GetByKey/{partitionKey}/{rowKey}")]
        public async Task<IActionResult> GetByKey(string partitionKey, string rowKey)
        {
            var location = await _locationService.GetByKeyAsync(partitionKey, rowKey);

            return location == null ? NotFound() : Ok(location);
        }

        [HttpDelete("Delete/{partitionKey}/{rowKey}")]
        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            Response response = await _locationService.DeleteAsync(partitionKey, rowKey);

            return response.IsError ? NotFound("Given keypair does not exist") : NoContent();
        }
    }
}
