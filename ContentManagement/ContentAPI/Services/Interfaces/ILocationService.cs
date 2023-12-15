using Azure;
using ContentAPI.Domain;
using ContentAPI.Domain.RequestModel;

namespace ContentAPI.Services.Interfaces
{
    public interface ILocationService
    {
        public Task<Location> CreateLocation(CreateLocationRequestModel model);
        public Task<Location> UpdateLocationAsync(UpdateLocationRequestModel model);
        public Task<Location> GetLocationByKeyAsync(string partitionKey, string rowKey);
        public Task<Response> DeleteLocationAsync(string partitionKey, string rowKey);
    }
}
