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
            string? connectionString = Environment.GetEnvironmentVariable("AZURE_CONNECTION");
            string containerName = "image-container";

            _blobContainerClient = new BlobContainerClient(connectionString, containerName);
            _blobContainerClient.CreateIfNotExists();
            _blobContainerClient.SetAccessPolicy(PublicAccessType.Blob);
        }

        public string AddJpgImage(string prefix, string image, string folderName = "")
        {
            if (string.IsNullOrEmpty(image))
            {
                return string.Empty;
            }

            string blobFileName = folderName + prefix + "-" + Guid.NewGuid().ToString() + DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") + ".jpg";

            BlobClient blobClient = _blobContainerClient.GetBlobClient(blobFileName);

            BlobUploadOptions options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = "image/jpeg" }
            };

            using (var stream = new MemoryStream(Convert.FromBase64String(image)))
            {
                blobClient.Upload(stream, options);
            }

            return blobClient.Uri.ToString();
        }

        public void DeleteImage(string blobName)
        {
            _blobContainerClient.GetBlobClient(blobName).Delete(snapshotsOption: DeleteSnapshotsOption.IncludeSnapshots);
        }
    }
}
