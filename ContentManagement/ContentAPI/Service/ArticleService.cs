using Azure;
using ContentAPI.DAL;
using ContentAPI.DAL.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Domain.DTO;
using ContentAPI.Service.Interfaces;
using System.Text.Json;

namespace ContentAPI.Service
{
    public class ArticleService : IArticleService
    {

        private readonly IArticleRepo<Article> _articleRepo;
        private readonly IBlobStorageService _blobService;
        public ArticleService(IArticleRepo<Article> articleRepo, IBlobStorageService blobService)
        {
            _articleRepo = articleRepo;
            _blobService = blobService;
        }

        public async Task<Article> Create(CreateArticleDTO DTO)
        {
            Article newArt = new Article
            {
                PartitionKey = DTO.ArticleType.ToString(),
                RowKey = Guid.NewGuid().ToString(),
                Title = DTO.Title,
                Subtitle = DTO.Subtitle,
                MainText = DTO.MainText
            };
            newArt.SerializedImageURLs = JsonSerializer.Serialize(_blobService.StoreNewImagesAndRetrieveBlobUrls(DTO.Base64Images, newArt));

            await _articleRepo.UpsertAsync(newArt);
            return newArt;
        }

        public async Task<Response> DeleteAsync(string partitionKey, string rowKey)
        {
            return await _articleRepo.DeleteAsync(partitionKey, rowKey);
        }

        public async Task<Article> GetByKeyAsync(string partitionKey, string rowKey)
        {
            return await _articleRepo.GetByKeyAsync(partitionKey, rowKey);
        }

        public async Task<Article> UpdateAsync(UpdateArticleDTO DTO)
        {
            Article updatedArt = new Article
            {
                PartitionKey = DTO.PartitionKey,
                RowKey = DTO.RowKey,
                ETag = DTO.ETag,
                Timestamp = DTO.Timestamp,
                Title = DTO.Title,
                Subtitle = DTO.Subtitle,
                MainText = DTO.MainText,
            };

            updatedArt.SerializedImageURLs = JsonSerializer.Serialize(_blobService.UpdateImagesAndRetrieveBlobUrls(DTO.OldSerializedImageURLs, DTO.Base64Images, updatedArt));

            return await _articleRepo.UpsertAsync(updatedArt);
        }
    }
}
