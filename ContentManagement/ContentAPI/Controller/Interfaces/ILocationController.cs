using ContentAPI.Domain;
using ContentAPI.Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ContentAPI.Controller.Interfaces
{
    public interface ILocationController
    {
        Task<IActionResult> GetLocationByKey(string partitionKey, string rowKey);
        Task<IActionResult> CreateLocation([FromBody] CreateLocationDTO locationDTO);
        //Task<IActionResult> UpdateLocation([FromBody] Location location);
        Task<IActionResult> DeleteLocation(string partitionKey, string rowKey);
    }
}
