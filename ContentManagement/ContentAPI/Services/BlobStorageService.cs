using Azure.Storage.Blobs;
using ContentAPI.DAL.Interfaces;
using ContentAPI.Services.Interfaces;

namespace ContentAPI.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IBlobStorageRepo _blobStorageRepo;

        public BlobStorageService(IBlobStorageRepo blobStorageRepo)
        {
            _blobStorageRepo = blobStorageRepo;
        }

        public BlobClient StoreImage(IFormFile image)
        {
            return _blobStorageRepo.StoreImage(image);
        }

        public string GetURL(BlobClient blobClient)
        {
            return _blobStorageRepo.GetURL(blobClient);
        }
    }
}
