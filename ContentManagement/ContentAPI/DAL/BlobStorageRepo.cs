using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ContentAPI.DAL.Interfaces;

namespace ContentAPI.DAL
{
    public class BlobStorageRepo : IBlobStorageRepo
    {
        private readonly BlobContainerClient _blobContainerClient;

        public BlobStorageRepo()
        {
            string connectionString = "UseDevelopmentStorage=true";
            string containerName = "image-container";

            _blobContainerClient = new BlobContainerClient(connectionString, containerName);
            _blobContainerClient.CreateIfNotExists();
            _blobContainerClient.SetAccessPolicy(PublicAccessType.Blob);
        }

        public string AddJpgImage(string image)
        {
            if (string.IsNullOrEmpty(image))
            {
                return string.Empty;
            }

            string blobFileName = Guid.NewGuid().ToString() + DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") + ".jpg";

            BlobClient blobClient = _blobContainerClient.GetBlobClient(blobFileName);

            BlobUploadOptions options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = "image/jpeg" }
            };

            byte[] imageBytes = Convert.FromBase64String(image);

            using (var stream = new MemoryStream(imageBytes))
            {
                blobClient.Upload(stream, options);
            }

            return blobClient.Uri.ToString();
        }

        public void DeleteImage(string blobName)
        {
            _blobContainerClient.GetBlobClient(blobName).DeleteIfExists();
        }
    }
}
