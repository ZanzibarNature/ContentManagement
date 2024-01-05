﻿using ContentAPI.Controllers.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Domain.DTO;
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

        [HttpGet("GetByKey/{partitionKey}/{rowKey}")]
        public async Task<IActionResult> GetByKey(string partitionKey, string rowKey)
        {
            Article art = await _articleService.GetByKeyAsync(partitionKey, rowKey);

            return art == null ? NotFound() : Ok(art);
        }
    }
}
