namespace CoreLibrary.Utilities.ObjectStorage.Base
{
    public interface IStorage
    {
        Task<List<(string fileName, string path)>> UploadAsync(string pathOrContainerName, List<string> imageAsBase64List);
        Task DeleteAsync(string pathOrContainerName, string fileName);
        List<string> GetFiles(string pathOrContainerName);
        bool HasFile(string pathOrContainerName, string fileName);
    }
}

