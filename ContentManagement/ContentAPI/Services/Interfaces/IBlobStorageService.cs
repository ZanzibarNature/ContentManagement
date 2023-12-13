using Azure.Storage.Blobs;

namespace ContentAPI.Services.Interfaces
{
    public interface IBlobStorageService
    {
        BlobClient? StoreImage(IFormFile image);
        string GetURL(BlobClient blobClient);
    }
}
