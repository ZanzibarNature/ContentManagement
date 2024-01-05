using ContentAPI.Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ContentAPI.Controllers.Interfaces
{
    public interface IArticleController
    {
        Task<IActionResult> Create([FromBody] CreateArticleDTO DTO);
        Task<IActionResult> Update([FromBody] UpdateArticleDTO DTO);
        Task<IActionResult> GetByKey(string partitionKey, string rowKey);
        Task<IActionResult> Delete(string partitionKey, string rowKey);
    }
}
