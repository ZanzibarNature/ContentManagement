using ContentAPI.Domain.DTO;
using ContentAPI.Domain;
using ContentAPI.Domain.RequestModel;

namespace ContentAPI.Services.Interfaces
{
    public interface ILocationService
    {
        public Task<Location> CreateLocation(LocationRequestModel model);
        public Task<Location> UpdateLocationAsync(Location updatedLocation, IFormFile? bannerImage, IFormFileCollection? additionalImages);
        public Task<Location> GetLocationByKeyAsync(string partitionKey, string rowKey);
        public Task DeleteLocationAsync(string partitionKey, string rowKey);
    }
}
