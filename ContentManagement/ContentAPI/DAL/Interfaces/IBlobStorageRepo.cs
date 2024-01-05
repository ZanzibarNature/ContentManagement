namespace ContentAPI.DAL.Interfaces
{
    public interface IBlobStorageRepo
    {
        string StoreJpgImage(string prefix, string image, string folderName = "");
        void DeleteImage(string blobName);
    }
}
