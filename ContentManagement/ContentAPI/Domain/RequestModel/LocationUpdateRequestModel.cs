using ContentAPI.Domain.DTO;

namespace ContentAPI.Domain.RequestModel
{
    public class LocationUpdateRequestModel
    {
        public UpdateLocationDTO? DTO { get; set; }
        public Location? OldLocation { get; set; }
    }
}
