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
            };

            // Add images
            string imageName = "BannerImage";
            if (locationDTO.Images.TryGetValue(imageName, out string retrievedValue))
            {
                newLoc.BannerImageURL = _blobService.AddJpgImage(retrievedValue);
                locationDTO.Images.Remove(imageName);
            }

            foreach (var image in locationDTO.Images)
            {
                newLoc.AdditionalImageURLs += _blobService.AddJpgImage(image.Value) + ",";
            }
            
            await _locationRepo.UpsertLocationAsync(newLoc);
            return newLoc;
        }

        //public async Task<Location> UpdateLocationAsync(UpdateLocationRequestModel model)
        //{
        //    Location updatedLocation = model.UpdatedLocation;

        //    if (updatedLocation.BannerImageURL != null)
        //    {
        //        updatedLocation.BannerImageURL = _blobService.AddJpgImage(model.BannerImage);
        //    }
        //    if (updatedLocation.AdditionalImageURL != null)
        //    {
        //        updatedLocation.AdditionalImageURL = _blobService.AddJpgImage(model.AdditionalImage);
        //    }

        //    return await _locationRepo.UpsertLocationAsync(updatedLocation);
        //}
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
