using BrunoZell.ModelBinding;
using Microsoft.AspNetCore.Mvc;

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
