using ContentAPI.Domain.DTO;
using ContentAPI.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ContentAPI.Controller.Interfaces
{
    public interface ILocationController
    {
        Task<IActionResult> GetLocationByKey(string partitionKey, string rowKey);
        //Task<IActionResult> CreateLocation([FromBody] CreateLocationDTO locationDTO, [FromForm] IFormFile? bannerImage, [FromForm] IFormFileCollection? additionalImages);
        //Task<IActionResult> UpdateLocation([FromBody] Location location, [FromForm] IFormFile? bannerImage, [FromForm] IFormFileCollection? additionalImages);
        Task<IActionResult> DeleteLocation(string partitionKey, string rowKey);
    }
}
