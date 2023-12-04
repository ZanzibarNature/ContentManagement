using Microsoft.AspNetCore.Mvc;

namespace ContentAPI.Controller
{
    public interface IContentController
    {
        Task<IActionResult> GetByKey(string partitionKey, string rowKey);
        Task<IActionResult> GetPageByPartitionKey(string partitionKey);
        Task<IActionResult> CreateLocation();
        Task<IActionResult> UpdateLocation();
        Task<IActionResult> DeleteLocation();
        Task<IActionResult> CreateProject();
        Task<IActionResult> UpdateProject();
        Task<IActionResult> DeleteProject();
        Task<IActionResult> CreatePost();
        Task<IActionResult> UpdatePost();
        Task<IActionResult> DeletePost();
    }
}
