using Azure;
using ContentAPI.DAL.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Domain.DTO;
using ContentAPI.Services.Interfaces;
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
                InvolvementHighlight = locationDTO.InvolvementHighlight,
                GoogleMapsURL = locationDTO.GoogleMapsURL
            };

            string blobFolderName = $"{newLoc.PartitionKey}{newLoc.RowKey}/";
            Dictionary<string, string> blobUrls = new Dictionary<string, string>();

            foreach (var image in locationDTO.Base64Images)
            {
                blobUrls.Add(image.Key, _blobService.AddJpgImage(image.Key, image.Value, blobFolderName));
            }

            newLoc.SerializedImageURLs = JsonSerializer.Serialize(blobUrls);
            newLoc.SerializedIconNames = JsonSerializer.Serialize(locationDTO.IconNames);

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
