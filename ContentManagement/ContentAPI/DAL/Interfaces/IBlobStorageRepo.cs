using Azure.Storage.Blobs;

namespace ContentAPI.DAL.Interfaces
{
    public interface IBlobStorageRepo
    {
        BlobClient? StoreImage(IFormFile image);
        string GetURL(BlobClient blobClient);
    }
}
