using CoreLibrary.Extensions;

namespace CoreLibrary.Utilities.ObjectStorage.MinIO;

public class MinIOStorage : IMinIOStorage
{
    public async Task<List<(string fileName, string path)>> UploadAsync(string pathOrContainerName, List<string> imageAsBase64List)
    {
        List<(string fileName, string path)> datas = new();
        
        foreach (string image in imageAsBase64List)
        {
            string imageName = $"{Guid.NewGuid().ToString()}.{image.GetBase64MimeType()}";
            
            string targetFilePath = Path.Combine(pathOrContainerName, imageName);
            
            datas.Add((imageName, targetFilePath));
            
            if (!Directory.Exists(pathOrContainerName))
            {
                Directory.CreateDirectory(pathOrContainerName);
            }
            byte[] binaryData = Convert.FromBase64String(image);
            await File.WriteAllBytesAsync(targetFilePath, binaryData);
        }
        
        return datas;
    }

    public Task DeleteAsync(string pathOrContainerName, string fileName)
    {
        throw new NotImplementedException();
    }

    public List<string> GetFiles(string pathOrContainerName)
    {
        throw new NotImplementedException();
    }

    public bool HasFile(string pathOrContainerName, string fileName)
    {
        throw new NotImplementedException();
    }
}