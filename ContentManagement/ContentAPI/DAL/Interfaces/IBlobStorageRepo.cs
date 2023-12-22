using Azure.Storage.Blobs;

namespace ContentAPI.DAL.Interfaces
{
    public interface IBlobStorageRepo
    {
        string AddJpgImage(string image);
        void DeleteImage(string blobName);
    }
}
