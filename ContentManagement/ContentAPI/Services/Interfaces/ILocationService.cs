using ContentAPI.Domain.DTO;
using ContentAPI.Domain;

namespace ContentAPI.Services.Interfaces
{
    public interface ILocationService
    {
        public Task<Location> CreateLocation(CreateLocationDTO locationDTO, IFormFile? bannerImage, IFormFileCollection? additionalImageURLs);
        public Task<Location> UpdateLocationAsync(Location updatedLocation, IFormFile? bannerImage, IFormFileCollection? additionalImages);
        public Task<Location> GetLocationByKeyAsync(string partitionKey, string rowKey);
        public Task DeleteLocationAsync(string partitionKey, string rowKey);
    }
}
