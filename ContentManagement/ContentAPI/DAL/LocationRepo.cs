using Azure;
using Azure.Data.Tables;
using ContentAPI.DAL.Interfaces;

namespace ContentAPI.DAL
{
    public class LocationRepo<T> : BaseRepo, ILocationRepo<T> where T : class, ITableEntity, new()
    {
        private readonly TableClient _tableClient;

        public LocationRepo(IConfiguration config) : base(config)
        {
            TableServiceClient serviceClient = new TableServiceClient(connectionString);
            serviceClient.CreateTableIfNotExists("locations");
            _tableClient = serviceClient.GetTableClient("locations");
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
