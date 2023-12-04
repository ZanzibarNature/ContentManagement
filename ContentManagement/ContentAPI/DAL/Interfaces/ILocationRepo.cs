using Azure.Data.Tables;

namespace ContentAPI.DAL.Interfaces
{
    public interface ILocationRepo<T> where T : class, ITableEntity, new()
    {
        Task<T> GetLocationByKeyAsync(string partitionKey, string rowKey);
        Task<T> UpsertLocationAsync(T location);
        Task DeleteLocationAsync(string partitionKey, string rowKey);
    }
}
