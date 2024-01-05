using ContentAPI.DAL.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Service.Interfaces;

namespace ContentAPI.Service
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IBlobStorageRepo _blobStorageRepo;

        public BlobStorageService(IBlobStorageRepo blobStorageRepo)
        {
            _blobStorageRepo = blobStorageRepo;
        }

        public string AddJpgImage(string prefix, string image, string folderName = "")
        {
            return _blobStorageRepo.AddJpgImage(prefix, image, folderName);
        }

        public void DeleteImage(string blobName)
        {
            _blobStorageRepo.DeleteImage(blobName);
        }

        public Dictionary<string, string> GetImageURLs(Dictionary<string, string> newBase64Images, ContentBase content)
        {
            string blobFolderName = $"{content.PartitionKey}{content.RowKey}/";
            Dictionary<string, string> blobUrls = new Dictionary<string, string>();

            foreach (var image in newBase64Images)
            {
                blobUrls.Add(image.Key, AddJpgImage(image.Key, image.Value, blobFolderName));
            }

            return blobUrls;
        }
    }
}
