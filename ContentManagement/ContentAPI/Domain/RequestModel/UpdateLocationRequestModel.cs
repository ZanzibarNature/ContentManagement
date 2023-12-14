using ContentAPI.Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using BrunoZell.ModelBinding;

namespace ContentAPI.Domain.RequestModel
{
    public class UpdateLocationRequestModel
    {
        [ModelBinder(BinderType = typeof(JsonModelBinder))]
        public Location? UpdatedLocation { get; set; }
        public IFormFile? BannerImage { get; set; }
        public IFormFile? AdditionalImage { get; set; }
    }
}
