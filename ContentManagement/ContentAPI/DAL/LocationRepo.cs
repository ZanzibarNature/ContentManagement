using Azure;
using Azure.Data.Tables;
using ContentAPI.DAL.Interfaces;

namespace ContentAPI.DAL
{
    public class LocationRepo<T> : ILocationRepo<T> where T : class, ITableEntity, new()
    {
        private readonly TableClient _tableClient;
        private readonly IConfiguration _config;

        public LocationRepo(IConfiguration config)
        {
            _config = config;
            TableServiceClient serviceClient = new TableServiceClient(Environment.GetEnvironmentVariable("AZURE_CONNECTION") ?? _config["AzureWebStorage"]);
            serviceClient.CreateTableIfNotExists("locations");
            _tableClient = serviceClient.GetTableClient("locations");
        }

        public async Task<Response> DeleteLocationAsync(string partitionKey, string rowKey)
        {
            return await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }
        public async Task<T> GetLocationByKeyAsync(string partitionKey, string rowKey)
        {
            return await _tableClient.GetEntityAsync<T>(partitionKey, rowKey);
        }
        public async Task<T> UpsertLocationAsync(T location)
        {
            await _tableClient.UpsertEntityAsync(location);
            return location;
        }
    }
}
