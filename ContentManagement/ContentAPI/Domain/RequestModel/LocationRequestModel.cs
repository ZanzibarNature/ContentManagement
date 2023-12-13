using ContentAPI.Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ContentAPI.Domain.RequestModel
{
    public class LocationRequestModel
    {
        public CreateLocationDTO? LocationDTO { get; set; }
        public IFormFile? BannerImage { get; set; }
        public IFormFile? AdditionalImage { get; set; }
    }
}
