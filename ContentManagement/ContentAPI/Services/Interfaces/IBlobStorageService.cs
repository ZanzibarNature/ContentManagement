using Azure.Storage.Blobs;

namespace ContentAPI.Services.Interfaces
{
    public interface IBlobStorageService
    {
        string AddJpgImage(string prefix, string image, string folderName = "");
        void DeleteImage(string blobName);
    }
}
