using Azure;
using ContentAPI.DAL.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Domain.DTO;
using ContentAPI.Service.Interfaces;
using System.Text.Json;

namespace ContentAPI.Service
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

        public async Task<Location> Create(CreateLocationDTO DTO)
        {
            Location newLoc = new Location
            {
                PartitionKey = "Location",
                RowKey = Guid.NewGuid().ToString(),
                Name = DTO.Name,
                Description = DTO.Description,
                Latitude = DTO.Latitude,
                Longitude = DTO.Longitude,
                InvolvementHighlight = DTO.InvolvementHighlight,
                GoogleMapsURL = DTO.GoogleMapsURL
            };

            newLoc.SerializedImageURLs = JsonSerializer.Serialize(_blobService.GetImageURLs(DTO.Base64Images, newLoc));
            newLoc.SerializedIconNames = JsonSerializer.Serialize(DTO.IconNames);

            await _locationRepo.UpsertAsync(newLoc);
            return newLoc;
        }
        public async Task<Location> UpdateAsync(UpdateLocationDTO DTO)
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
                InvolvementHighlight = DTO.InvolvementHighlight,
                GoogleMapsURL = DTO.GoogleMapsURL,
                SerializedImageURLs = DTO.OldSerializedImageURLs
            };

            Dictionary<string, string> imageURLs = JsonSerializer.Deserialize<Dictionary<string, string>>(updatedLoc.SerializedImageURLs);

            // Update images in Blob
            foreach (var image in DTO.Base64Images)
            {
                if (imageURLs.ContainsKey(image.Key))
                {
                    Uri uri = new Uri(imageURLs[image.Key]);
                    string blobToDelete = Path.Combine(uri.Segments[3..]).Replace('\\', '/').Trim('/', '\\');
                    _blobService.DeleteImage(blobToDelete);
                }
                imageURLs[image.Key] = _blobService.AddJpgImage(image.Key, image.Value, $"{updatedLoc.PartitionKey}{updatedLoc.RowKey}/");
            }

            updatedLoc.SerializedImageURLs = JsonSerializer.Serialize(imageURLs);
            updatedLoc.SerializedIconNames = JsonSerializer.Serialize(DTO.IconNames);

            return await _locationRepo.UpsertAsync(updatedLoc);
        }
        public async Task<Location> GetByKeyAsync(string partitionKey, string rowKey)
        {
            return await _locationRepo.GetByKeyAsync(partitionKey, rowKey);
        }
        public async Task<Response> DeleteAsync(string partitionKey, string rowKey)
        {
            return await _locationRepo.DeleteAsync(partitionKey, rowKey);
        }
    }
}
