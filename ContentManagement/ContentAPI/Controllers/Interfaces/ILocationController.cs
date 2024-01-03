using ContentAPI.Domain.DTO;
using ContentAPI.Domain.RequestModel;
using Microsoft.AspNetCore.Mvc;

namespace ContentAPI.Controllers.Interfaces
{
    public interface ILocationController
    {
        Task<IActionResult> GetLocationByKey(string partitionKey, string rowKey);
        Task<IActionResult> CreateLocation([FromBody] CreateLocationDTO locationDTO);
        //Task<IActionResult> UpdateLocation([FromBody] LocationUpdateRequestModel model);
        Task<IActionResult> DeleteLocation(string partitionKey, string rowKey);
    }
}
