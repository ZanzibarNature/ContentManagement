using ContentAPI.Domain;

namespace ContentAPI.Service.Interfaces
{
    public interface IBlobStorageService
    {
        string AddJpgImage(string prefix, string image, string folderName = "");
        void DeleteImage(string blobName);
        Dictionary<string, string> GetImageURLs(Dictionary<string, string> newBase64Images, ContentBase content);
    }
}
