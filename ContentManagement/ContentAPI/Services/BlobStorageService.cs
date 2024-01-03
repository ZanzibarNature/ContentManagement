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

        public string AddJpgImage(string prefix, string image, string folderName = "")
        {
            return _blobStorageRepo.AddJpgImage(prefix, image, folderName);
        }

        public void DeleteImage(string blobName)
        {
            _blobStorageRepo.DeleteImage(blobName);
        }
    }
}
