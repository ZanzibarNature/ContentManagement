using ContentAPI.Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ContentAPI.Controllers.Interfaces
{
    public interface IArticleController
    {
        Task<IActionResult> Create([FromBody] CreateArticleDTO DTO);
        Task<IActionResult> GetByKey(string partitionKey, string rowKey);
    }
}
