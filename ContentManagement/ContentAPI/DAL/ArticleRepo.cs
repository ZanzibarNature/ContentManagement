using Azure;
using Azure.Data.Tables;
using ContentAPI.DAL.Interfaces;

namespace ContentAPI.DAL
{
    public class ArticleRepo<T> : BaseRepo, IArticleRepo<T> where T : class, ITableEntity, new()
    {
        private readonly TableClient _tableClient;
        private readonly string _tableName = "articles";

        public ArticleRepo(IConfiguration config) : base(config)
        {
            TableServiceClient serviceClient = new TableServiceClient(connectionString);
            serviceClient.CreateTableIfNotExists(_tableName);
            _tableClient = serviceClient.GetTableClient(_tableName);
        }
        public async Task<Response> DeleteAsync(string partitionKey, string rowKey)
        {
            return await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }
        public async Task<T> GetByKeyAsync(string partitionKey, string rowKey)
        {
            return await _tableClient.GetEntityAsync<T>(partitionKey, rowKey);
        }
        public async Task<Tuple<string, IEnumerable<T>>?> GetPage(string? continuationToken, int? maxPerPage)
        {
            if (maxPerPage <= 0 || maxPerPage >= 25)
            {
                maxPerPage = 10;
            }
            IList<T> results = new List<T>();
            AsyncPageable<T> locs = _tableClient.QueryAsync<T>(maxPerPage: maxPerPage);
            await foreach (var page in locs.AsPages(continuationToken))
            {
                var pageCollection = Tuple.Create<string, IEnumerable<T>>(page.ContinuationToken, page.Values);
                return pageCollection;
            }
            return null;
        }
        public async Task<T> UpsertAsync(T location)
        {
            await _tableClient.UpsertEntityAsync(location);
            return location;
        }
    }
}
