using ContentAPI.Domain.DTO;
using ContentAPI.Domain;
using Microsoft.AspNetCore.Mvc;
using ContentAPI.Domain.RequestModel;

namespace ContentAPI.Controller.Interfaces
{
    public interface ILocationController
    {
        Task<IActionResult> GetLocationByKey(string partitionKey, string rowKey);
        Task<IActionResult> CreateLocation([FromForm] CreateLocationRequestModel model);
        Task<IActionResult> UpdateLocation([FromForm] UpdateLocationRequestModel model);
        Task<IActionResult> DeleteLocation(string partitionKey, string rowKey);
    }
}
