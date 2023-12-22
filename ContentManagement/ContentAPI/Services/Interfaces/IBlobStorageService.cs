using Azure.Storage.Blobs;

namespace ContentAPI.Services.Interfaces
{
    public interface IBlobStorageService
    {
        string AddJpgImage(string image);
        void DeleteImage(string blobName);
    }
}
