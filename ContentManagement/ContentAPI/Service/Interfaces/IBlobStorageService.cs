using ContentAPI.Domain;

namespace ContentAPI.Service.Interfaces
{
    public interface IBlobStorageService
    {
        string StoreJpgImage(string prefix, string image, string folderName = "");
        void DeleteImage(string blobName);
        Dictionary<string, string> StoreNewImagesAndRetrieveBlobUrls(Dictionary<string, string> newBase64Images, ContentBase content);
        Dictionary<string, string> UpdateImagesAndRetrieveBlobUrls(string oldImageURLs, Dictionary<string, string> newBase64Images, ContentBase content);
    }
}
