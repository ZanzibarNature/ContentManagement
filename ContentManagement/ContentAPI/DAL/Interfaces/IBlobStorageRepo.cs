using Azure.Storage.Blobs;

namespace ContentAPI.DAL.Interfaces
{
    public interface IBlobStorageRepo
    {
        string? AddJpgImage(IFormFile image);
    }
}
