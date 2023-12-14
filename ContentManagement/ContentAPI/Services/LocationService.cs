using ContentAPI.DAL.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Domain.DTO;
using ContentAPI.Domain.RequestModel;
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

        public async Task<Location> CreateLocation(CreateLocationRequestModel model)
        {
            Location newLoc = new Location
            {
                PartitionKey = "Location",
                RowKey = Guid.NewGuid().ToString(),
                Name = model.LocationDTO.Name,
                Description = model.LocationDTO.Description,
                Latitude = model.LocationDTO.Latitude,
                Longitude = model.LocationDTO.Longitude,
                BannerImageURL = _blobService.AddJpgImage(model.BannerImage),
                AdditionalImageURL = _blobService.AddJpgImage(model.AdditionalImage)
            };
            
            await _locationRepo.UpsertLocationAsync(newLoc);
            return newLoc;
        }

        public async Task<Location> UpdateLocationAsync(UpdateLocationRequestModel model)
        {
            Location updatedLocation = model.UpdatedLocation;

            if (updatedLocation.BannerImageURL != null)
            {
                updatedLocation.BannerImageURL = _blobService.AddJpgImage(model.BannerImage);
            }
            if (updatedLocation.AdditionalImageURL != null)
            {
                updatedLocation.AdditionalImageURL = _blobService.AddJpgImage(model.AdditionalImage);
            }

            return await _locationRepo.UpsertLocationAsync(updatedLocation);
        }
        public async Task<Location> GetLocationByKeyAsync(string partitionKey, string rowKey)
        {
            return await _locationRepo.GetLocationByKeyAsync(partitionKey, rowKey);
        }
        public async Task DeleteLocationAsync(string partitionKey, string rowKey)
        {
            await _locationRepo.DeleteLocationAsync(partitionKey, rowKey);
        }
    }
}
