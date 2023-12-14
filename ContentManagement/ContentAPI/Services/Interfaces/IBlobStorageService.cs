using Azure.Storage.Blobs;

namespace ContentAPI.Services.Interfaces
{
    public interface IBlobStorageService
    {
        string AddJpgImage(IFormFile image);
    }
}
