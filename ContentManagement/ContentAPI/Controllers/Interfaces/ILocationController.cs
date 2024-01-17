using ContentAPI.Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ContentAPI.Controllers.Interfaces
{
    public interface ILocationController
    {
        Task<IActionResult> GetByKey(string partitionKey, string rowKey);
        Task<IActionResult> GetPage(string? continuationToken, int? maxPerPage);
        Task<IActionResult> Create([FromBody] CreateLocationDTO DTO);
        Task<IActionResult> Update([FromBody] UpdateLocationDTO DTO);
        Task<IActionResult> Delete(string partitionKey, string rowKey);
    }
}
