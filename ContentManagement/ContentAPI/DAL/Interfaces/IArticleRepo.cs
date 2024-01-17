using Azure;
using Azure.Data.Tables;

namespace ContentAPI.DAL.Interfaces
{
    public interface IArticleRepo<T> where T : class, ITableEntity, new()
    {
        Task<T> GetByKeyAsync(string partitionKey, string rowKey);
        Task<Tuple<string, IEnumerable<T>>?> GetPage(string? continuationToken, int? maxPerPage);
        Task<T> UpsertAsync(T location);
        Task<Response> DeleteAsync(string partitionKey, string rowKey);
    }
}
