using Azure;
using ContentAPI.Domain;
using ContentAPI.Domain.DTO;

namespace ContentAPI.Services.Interfaces
{
    public interface ILocationService
    {
        public Task<Location> CreateLocation(CreateLocationDTO locationDTO);
        //public Task<Location> UpdateLocationAsync(UpdateLocationRequestModel model);
        public Task<Location> GetLocationByKeyAsync(string partitionKey, string rowKey);
        public Task<Response> DeleteLocationAsync(string partitionKey, string rowKey);
    }
}
