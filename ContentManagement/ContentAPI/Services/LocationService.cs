using Azure;
using ContentAPI.DAL.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Domain.DTO;
using ContentAPI.Services.Interfaces;

namespace ContentAPI.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepo<Location> _locationRepo;
        private readonly IBlobStorageService _blobService;
        public LocationService(ILocationRepo<Location> locationRepo, IBlobStorageService blobService)
        {
            _locationRepo = locationRepo;
            _blobService = blobService;
        }

        public async Task<Location> CreateLocation(CreateLocationDTO locationDTO)
        {
            Location newLoc = new Location
            {
                PartitionKey = "Location",
                RowKey = Guid.NewGuid().ToString(),
                Name = locationDTO.Name,
                Description = locationDTO.Description,
                Latitude = locationDTO.Latitude,
                Longitude = locationDTO.Longitude,
                ImageURLs = new Dictionary<string, string>()
            };

            // Add image urls
            foreach (var image in locationDTO.Base64Images)
            {
                newLoc.ImageURLs.Add(image.Key, _blobService.AddJpgImage(image.Value));
            }

            await _locationRepo.UpsertLocationAsync(newLoc);
            return newLoc;
        }

        public async Task<Location> UpdateLocationAsync(UpdateLocationDTO DTO)
        {
            Location updatedLocation = new Location
            {
                PartitionKey = DTO.PartitionKey,
                RowKey = DTO.RowKey,
                ETag = DTO.ETag,
                Timestamp = DTO.Timestamp,
                Name = DTO.Name,
                Description = DTO.Description,
                Latitude = DTO.Latitude,
                Longitude = DTO.Longitude,
            };

            if (DTO.Base64Images.TryGetValue("BannerImage", out string bannerImage) && bannerImage != null)
            {
                updatedLocation.BannerImageURL = _blobService.AddJpgImage(bannerImage);
            }
            foreach (var image in DTO.Base64Images)
            {
                updatedLocation.AdditionalImageURLs +=
            }

            return await _locationRepo.UpsertLocationAsync(updatedLocation);
        }
        public async Task<Location> GetLocationByKeyAsync(string partitionKey, string rowKey)
        {
            return await _locationRepo.GetLocationByKeyAsync(partitionKey, rowKey);
        }
        public async Task<Response> DeleteLocationAsync(string partitionKey, string rowKey)
        {
            return await _locationRepo.DeleteLocationAsync(partitionKey, rowKey);
        }
    }
}
