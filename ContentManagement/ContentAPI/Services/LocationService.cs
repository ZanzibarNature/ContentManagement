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

            foreach (var image in locationDTO.Base64Images)
            {
                newLoc.ImageURLs.Add(image.Key, _blobService.AddJpgImage(image.Value));
            }

            await _locationRepo.UpsertLocationAsync(newLoc);
            return newLoc;
        }

        public async Task<Location> UpdateLocationAsync(UpdateLocationDTO DTO, Location oldLocation)
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
                ImageURLs = oldLocation.ImageURLs
            };

            // Update images in Blob
            foreach (var image in DTO.Base64Images)
            {
                if (updatedLocation.ImageURLs.ContainsKey(image.Key))
                {
                    _blobService.DeleteImage(image.Value);
                }
                updatedLocation.ImageURLs[image.Key] = _blobService.AddJpgImage(image.Value);
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
