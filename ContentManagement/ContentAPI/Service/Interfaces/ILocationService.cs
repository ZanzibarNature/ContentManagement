using Azure;
using ContentAPI.Domain;
using ContentAPI.Domain.DTO;

namespace ContentAPI.Service.Interfaces
{
    public interface ILocationService
    {
        public Task<Location> Create(CreateLocationDTO DTO);
        public Task<Location> UpdateAsync(UpdateLocationDTO DTO);
        public Task<Location> GetByKeyAsync(string partitionKey, string rowKey);
        public Task<Response> DeleteAsync(string partitionKey, string rowKey);
    }
}
