using Azure;
using Azure.Data.Tables;
using ContentAPI.Domain;

namespace ContentAPI.DAL.Interfaces
{
    public interface ILocationRepo<T> where T : class, ITableEntity, new()
    {
        Task<T> GetByKeyAsync(string partitionKey, string rowKey);
        Task<Tuple<string, IEnumerable<T>>?> GetPage(string? continuationToken, int? maxPerPage);
        Task<T> UpsertAsync(T location);
        Task<Response> DeleteAsync(string partitionKey, string rowKey);
    }
}
