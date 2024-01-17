using Azure;
using ContentAPI.Controllers.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Domain.DTO;
using ContentAPI.Middleware;
using ContentAPI.Service;
using ContentAPI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContentAPI.Controllers
{
    [Route("api/Content/Articles")]
    [ApiController]
    public class ArticleController : ControllerBase, IArticleController
    {
        private readonly IArticleService _articleService;
        private readonly IBlobStorageService _blobService;
        public ArticleController(IArticleService articleService, IBlobStorageService blobService)
        {
            _articleService = articleService;
            _blobService = blobService;
        }

        [HttpGet("GetByKey/{partitionKey}/{rowKey}")]
        public async Task<IActionResult> GetByKey(string partitionKey, string rowKey)
        {
            Article art = await _articleService.GetByKeyAsync(partitionKey, rowKey);

            return art == null ? NotFound() : Ok(art);
        }

        [CustomAuth]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateArticleDTO DTO)
        {
            if (DTO == null)
            {
                return BadRequest("ArticleDTO object is null or invalid");
            }

            Article newArt = await _articleService.Create(DTO);

            return CreatedAtAction(nameof(GetByKey), new { partitionKey = newArt.PartitionKey, rowKey = newArt.RowKey }, newArt);
        }

        [CustomAuth]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateArticleDTO DTO)
        {
            try
            {
                if (DTO == null)
                {
                    return BadRequest("The given DTO is null or invalid");
                }

                Article updatedArt = await _articleService.UpdateAsync(DTO);

                return Ok(updatedArt);
            }
            catch (RequestFailedException ex)
            {
                return StatusCode(ex.Status, ex.Message);
            }
           
        }

        [CustomAuth]
        [HttpDelete("Delete/{partitionKey}/{rowKey}")]
        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            Response response = await _articleService.DeleteAsync(partitionKey, rowKey);

            return response.IsError ? NotFound("Given keypair does not exist") : Ok("Article succesfully deleted");
        }

        [HttpGet("GetPage")]
        public async Task<IActionResult> GetPage(string? continuationToken, int? maxPerPage)
        {
            Tuple<string, IEnumerable<Article>>? arts = await _articleService.GetPage(continuationToken, maxPerPage);

            return arts == null ? NotFound("No locations found") : Ok(arts);
        }
    }
}
