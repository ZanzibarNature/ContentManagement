using ContentAPI.Domain.DTO;
using ContentAPI.Domain;

namespace ContentAPI.Services.Interfaces
{
    public interface ILocationService
    {
        public Task<Location> CreateLocation(CreateLocationDTO locationDTO);

        public Task<Location> GetLocationByKeyAsync(string partitionKey, string rowKey);
    }
}
