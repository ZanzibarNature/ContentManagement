using ContentAPI.Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using BrunoZell.ModelBinding;

namespace ContentAPI.Domain.RequestModel
{
    public class CreateLocationRequestModel
    {
        [ModelBinder(BinderType = typeof(JsonModelBinder))]
        public CreateLocationDTO? LocationDTO { get; set; }
        public IFormFile? BannerImage { get; set; }
        public IFormFile? AdditionalImage { get; set; }
    }
}
