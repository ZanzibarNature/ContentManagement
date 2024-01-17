using ContentAPI.DAL.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Service.Interfaces;
using System.Text.Json;

namespace ContentAPI.Service
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IBlobStorageRepo _blobStorageRepo;

        public BlobStorageService(IBlobStorageRepo blobStorageRepo)
        {
            _blobStorageRepo = blobStorageRepo;
        }

        public string StoreJpgImage(string prefix, string image, string folderName = "")
        {
            return _blobStorageRepo.StoreJpgImage(prefix, image, folderName);
        }

        public void DeleteImage(string blobName)
        {
            _blobStorageRepo.DeleteImage(blobName);
        }

        public Dictionary<string, string> StoreNewImagesAndRetrieveBlobUrls(Dictionary<string, string> newBase64Images, ContentBase content)
        {
            Dictionary<string, string> blobUrls = new Dictionary<string, string>();

            foreach (var image in newBase64Images)
            {
                blobUrls.Add(image.Key, StoreJpgImage(image.Key, image.Value, $"{content.PartitionKey}{content.RowKey}/"));
            }

            return blobUrls;
        }

        public Dictionary<string, string> UpdateImagesAndRetrieveBlobUrls(string oldImageURLs, Dictionary<string, string> newBase64Images, ContentBase content)
        {
            Dictionary<string, string> imageURLs = JsonSerializer.Deserialize<Dictionary<string, string>>(oldImageURLs);

            foreach (var newImage in newBase64Images)
            {
                if (imageURLs.ContainsKey(newImage.Key))
                {
                    Uri uri = new Uri(imageURLs[newImage.Key]);
                    string blobToDelete = Path.Combine(uri.Segments[3..]).Replace('\\', '/').Trim('/', '\\');
                    DeleteImage(blobToDelete);
                }
                imageURLs[newImage.Key] = StoreJpgImage(newImage.Key, newImage.Value, $"{content.PartitionKey}{content.RowKey}/");
            }

            return imageURLs;
        }
    }
}
