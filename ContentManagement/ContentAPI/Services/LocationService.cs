using Azure;
using ContentAPI.DAL.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Domain.DTO;
using ContentAPI.Services.Interfaces;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.Json;

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
                //ImageURLs = new Dictionary<string, string>()
            };

            string blobFolderName = $"{newLoc.PartitionKey}{newLoc.RowKey}/";
            Dictionary<string, string> blobUrls = new Dictionary<string, string>();

            foreach (var image in locationDTO.Base64Images)
            {
                //newLoc.ImageURLs.Add(image.Key, _blobService.AddJpgImage(image.Value));
                blobUrls.Add(image.Key, _blobService.AddJpgImage(image.Key, image.Value, blobFolderName));
            }

            newLoc.SerializedImageURLs = JsonSerializer.Serialize(blobUrls);

            await _locationRepo.UpsertLocationAsync(newLoc);
            return newLoc;
        }
        public async Task<Location> UpdateLocationAsync(UpdateLocationDTO DTO)
        {
            Location updatedLoc = new Location
            {
                PartitionKey = DTO.PartitionKey,
                RowKey = DTO.RowKey,
                ETag = DTO.ETag,
                Timestamp = DTO.Timestamp,
                Name = DTO.Name,
                Description = DTO.Description,
                Latitude = DTO.Latitude,
                Longitude = DTO.Longitude,
                SerializedImageURLs = DTO.OldSerializedImageURLs
            };

            Dictionary<string, string> existingImages = JsonSerializer.Deserialize<Dictionary<string, string>>(updatedLoc.SerializedImageURLs);
            Dictionary<string, string> newImageURLs = new Dictionary<string, string>();

            // Update images in Blob
            foreach (var image in DTO.Base64Images)
            {
                if (existingImages.ContainsKey(image.Key))
                {
                    Uri uri = new Uri(existingImages[image.Key]);
                    string blobToDelete = Path.Combine(uri.Segments[3..]).Replace('\\', '/').Trim('/', '\\');
                    _blobService.DeleteImage(blobToDelete);
                }
                newImageURLs[image.Key] = _blobService.AddJpgImage(image.Key, image.Value, $"{updatedLoc.PartitionKey}{updatedLoc.RowKey}/");
            }

            updatedLoc.SerializedImageURLs = JsonSerializer.Serialize(newImageURLs);

            return await _locationRepo.UpsertLocationAsync(updatedLoc);
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
