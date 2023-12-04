using ContentAPI.Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ContentAPI.Controller
{
    public interface IContentController
    {
        Task<IActionResult> GetLocationByKey(string partitionKey, string rowKey);
        //Task<IActionResult> GetProjectByKey(string partitionKey, string rowKey);
        //Task<IActionResult> GetPostByKey(string partitionKey, string rowKey);
        Task<IActionResult> CreateLocation([FromBody] CreateLocationDTO locationDTO);
        //Task<IActionResult> UpdateLocation();
        //Task<IActionResult> DeleteLocation();
        //Task<IActionResult> CreateProject();
        //Task<IActionResult> UpdateProject();
        //Task<IActionResult> DeleteProject();
        //Task<IActionResult> CreatePost();
        //Task<IActionResult> UpdatePost();
        //Task<IActionResult> DeletePost();
    }
}
