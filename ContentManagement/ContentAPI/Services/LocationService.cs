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

        /// <summary>
        /// This method creates a new Location object. If given, it also adds the banner image url and/or additional image url collection.
        /// If the given image is null, the returned URL becomes an empty string.
        /// If there a no addtional images, the returned collection is an empty list.
        /// </summary>
        public async Task<Location> CreateLocation(CreateLocationDTO locationDTO, IFormFile? bannerImage, IFormFileCollection? additionalImages)
        {
            Location newLoc = new Location
            {
                PartitionKey = "Location",
                RowKey = Guid.NewGuid().ToString(),
                Name = locationDTO.Name,
                Description = locationDTO.Description,
                Latitude = locationDTO.Latitude,
                Longitude = locationDTO.Longitude,
                BannerImageURL = _blobService.GetURL(_blobService.StoreImage(bannerImage)),
                AdditionalImageURLs = new List<string>()
            };

            if (additionalImages != null && additionalImages.Any())
            {
                foreach (var image in additionalImages)
                {
                    newLoc.AdditionalImageURLs.Add(_blobService.GetURL(_blobService.StoreImage(image)));
                }
            }
            
            await _locationRepo.UpsertLocationAsync(newLoc);
            return newLoc;
        }

        /// <summary>
        /// This method updates the given Location object. If given, it also updates the banner image url and/or additional image url collection.
        /// If the given image is null, the returned URL becomes an empty string.
        /// </summary>
        public async Task<Location> UpdateLocationAsync(Location updatedLocation, IFormFile? bannerImage, IFormFileCollection? additionalImages)
        {
            if (bannerImage != null)
            {
                updatedLocation.BannerImageURL = _blobService.GetURL(_blobService.StoreImage(bannerImage));
            }
            if (additionalImages != null && additionalImages.Any())
            {
                foreach (var image in additionalImages)
                {
                    updatedLocation.AdditionalImageURLs.Add(_blobService.GetURL(_blobService.StoreImage(image)));
                }
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
