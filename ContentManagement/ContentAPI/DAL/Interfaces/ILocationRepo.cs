using Azure;
using Azure.Data.Tables;

namespace ContentAPI.DAL.Interfaces
{
    public interface ILocationRepo<T> where T : class, ITableEntity, new()
    {
        Task<T> GetByKeyAsync(string partitionKey, string rowKey);
        Task<T> UpsertAsync(T location);
        Task<Response> DeleteAsync(string partitionKey, string rowKey);
    }
}
