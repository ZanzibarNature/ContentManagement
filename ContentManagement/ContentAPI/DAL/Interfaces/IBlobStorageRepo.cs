using Azure.Storage.Blobs;

namespace ContentAPI.DAL.Interfaces
{
    public interface IBlobStorageRepo
    {
        BlobClient? StoreImageAsJpg(IFormFile image);
        string GetURL(BlobClient blobClient);
    }
}
