using Azure;
using ContentAPI.Domain;
using ContentAPI.Domain.DTO;

namespace ContentAPI.Service.Interfaces
{
    public interface IArticleService
    {
        public Task<Article> Create(CreateArticleDTO DTO);
        public Task<Article> UpdateAsync(UpdateArticleDTO DTO);
        public Task<Article> GetByKeyAsync(string partitionKey, string rowKey);
        public Task<Response> DeleteAsync(string partitionKey, string rowKey);
    }
}
