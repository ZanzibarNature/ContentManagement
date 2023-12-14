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

        public string? AddJpgImage(IFormFile image)
        {
            if (image == null)
            {
                return null;
            }

            string blobFileName = Guid.NewGuid().ToString() + DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") + ".jpg";

            BlobClient blobClient = _blobContainerClient.GetBlobClient(blobFileName);

            BlobUploadOptions options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = "image/jpeg" }
            };

            blobClient.Upload(image.OpenReadStream(), options);

            return blobClient.Uri.ToString();
        }
    }
}
