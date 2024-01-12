using Azure;
using Azure.Data.Tables;
using ContentAPI.DAL.Interfaces;

namespace ContentAPI.DAL
{
    public class ArticleRepo<T> : BaseRepo, IArticleRepo<T> where T : class, ITableEntity, new()
    {
        private readonly TableClient _tableClient;

        public ArticleRepo(IConfiguration config) : base(config)
        {
            TableServiceClient serviceClient = new TableServiceClient(connectionString);
            serviceClient.CreateTableIfNotExists("articles");
            _tableClient = serviceClient.GetTableClient("articles");
        }
        public async Task<Response> DeleteAsync(string partitionKey, string rowKey)
        {
            return await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }
        public async Task<T> GetByKeyAsync(string partitionKey, string rowKey)
        {
            return await _tableClient.GetEntityAsync<T>(partitionKey, rowKey);
        }
        public async Task<T> UpsertAsync(T location)
        {
            await _tableClient.UpsertEntityAsync(location);
            return location;
        }
    }
}
