using ContentAPI.DAL.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Domain.DTO;
using ContentAPI.Services.Interfaces;

namespace ContentAPI.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepo<Location> _locationRepo;
        public LocationService(ILocationRepo<Location> locationRepo)
        {
            _locationRepo = locationRepo;
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
                Longitude = locationDTO.Longitude
            };

            await _locationRepo.UpsertLocationAsync(newLoc);
            return newLoc;
        }
        public async Task<Location> UpdateLocationAsync(Location updatedLocation)
        {
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
