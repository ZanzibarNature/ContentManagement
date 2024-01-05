namespace ContentAPI.DAL.Interfaces
{
    public interface IBlobStorageRepo
    {
        string AddJpgImage(string prefix, string image, string folderName = "");
        void DeleteImage(string blobName);
    }
}
